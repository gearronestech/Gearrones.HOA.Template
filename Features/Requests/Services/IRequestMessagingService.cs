using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Requests.ViewModels;

namespace GearrOnes.HOA.Template.Features.Requests.Services;

[HoaFeature("FullManagement", 2)]
public interface IRequestMessagingService
{
    Task<IReadOnlyList<RequestSummaryViewModel>> GetVisibleRequestsAsync(string userId, string role, CancellationToken cancellationToken = default);
    Task<RequestDetailViewModel?> GetRequestDetailsAsync(int requestId, string userId, string role, CancellationToken cancellationToken = default);
    Task AddMessageAsync(CreateRequestMessageInputModel input, string userId, string role, CancellationToken cancellationToken = default);
    Task AddAttachmentAsync(UploadRequestAttachmentInputModel input, string userId, string role, CancellationToken cancellationToken = default);
    Task<RequestAttachmentFileResult?> GetAttachmentAsync(int attachmentId, string userId, string role, CancellationToken cancellationToken = default);
}

public class RequestSummaryViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class RequestAttachmentFileResult
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
    public Stream ContentStream { get; set; } = Stream.Null;
}