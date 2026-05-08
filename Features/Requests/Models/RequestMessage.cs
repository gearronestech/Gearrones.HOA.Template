namespace GearrOnes.HOA.Template.Features.Requests.Models;

public class RequestMessage
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
    public bool IsInternalNote { get; set; }

    public Request? Request { get; set; }
    public ApplicationUser? User { get; set; }
}