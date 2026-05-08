using GearrOnes.HOA.Template.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace GearrOnes.HOA.Template.Features.Requests.Services;

[HoaFeature("FullManagement", 2)]
public interface IFileStorageService
{
    Task<StoredFileResult> SaveRequestAttachmentAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadAsync(string relativePath, CancellationToken cancellationToken = default);
}

public sealed class StoredFileResult
{
    public string StoredFileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
}