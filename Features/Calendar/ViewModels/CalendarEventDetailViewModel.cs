using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Calendar.ViewModels;

[HoaFeature("FullManagement", 2)]
public class CalendarEventDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public int? LinkedPostId { get; set; }
    public DateTime CreatedUtc { get; set; }
}