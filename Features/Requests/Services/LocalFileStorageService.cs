using GearrOnes.HOA.Template.Core.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GearrOnes.HOA.Template.Features.Requests.Services;

[HoaFeature("FullManagement", 2)]
public sealed class LocalFileStorageService : IFileStorageService
{
    private static readonly HashSet<string> AllowedExtensions = [".pdf", ".png", ".jpg", ".jpeg", ".doc", ".docx", ".txt"];
    private const long MaxFileBytes = 10 * 1024 * 1024;

    private readonly string _rootFolder;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        _rootFolder = Path.Combine(env.ContentRootPath, "App_Data", "request-attachments");
        Directory.CreateDirectory(_rootFolder);
    }

    public async Task<StoredFileResult> SaveRequestAttachmentAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file.Length <= 0) throw new InvalidOperationException("File is empty.");
        if (file.Length > MaxFileBytes) throw new InvalidOperationException("File exceeds the 10 MB size limit.");

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext.ToLowerInvariant()))
            throw new InvalidOperationException("File type is not allowed.");

        var storedName = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
        var fullPath = Path.GetFullPath(Path.Combine(_rootFolder, storedName));
        if (!fullPath.StartsWith(_rootFolder, StringComparison.Ordinal)) throw new InvalidOperationException("Invalid file path.");

        await using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream, cancellationToken);

        return new StoredFileResult
        {
            StoredFileName = storedName,
            RelativePath = storedName,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            FileSize = file.Length
        };
    }

    public Task<Stream> OpenReadAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var safeName = Path.GetFileName(relativePath);
        var fullPath = Path.GetFullPath(Path.Combine(_rootFolder, safeName));
        if (!fullPath.StartsWith(_rootFolder, StringComparison.Ordinal) || !File.Exists(fullPath))
            throw new FileNotFoundException();

        Stream stream = File.OpenRead(fullPath);
        return Task.FromResult(stream);
    }
}
