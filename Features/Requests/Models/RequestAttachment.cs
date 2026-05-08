using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Requests.Models;

[HoaFeature("FullManagement", 2)]
public class RequestAttachment
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string UploadedByUserId { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedUtc { get; set; }

    public Request? Request { get; set; }
}
