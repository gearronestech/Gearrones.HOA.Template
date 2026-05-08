namespace GearrOnes.HOA.Template.Features.Requests.Models;

public class Request
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string HomeownerUserId { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
    public List<RequestMessage> Messages { get; set; } = new();
    public List<RequestAttachment> Attachments { get; set; } = new();
}