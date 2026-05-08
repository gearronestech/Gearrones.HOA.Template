using GearrOnes.HOA.Template.Features.Requests.Models;

namespace GearrOnes.HOA.Template.Features.Requests.ViewModels;

public class RequestDetailViewModel
{
    public Request Request { get; set; } = new();
    public IReadOnlyList<RequestMessageTimelineItemViewModel> Timeline { get; set; } = [];
    public bool CanCreateInternalNote { get; set; }
}

public class RequestMessageTimelineItemViewModel
{
    public int MessageId { get; set; }
    public string UserDisplayName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
    public bool IsInternalNote { get; set; }
}

public class CreateRequestMessageInputModel
{
    public int RequestId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsInternalNote { get; set; }
}