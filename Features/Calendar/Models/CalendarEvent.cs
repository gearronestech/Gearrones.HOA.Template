using System.ComponentModel.DataAnnotations;
using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Calendar.Models;

[HoaFeature("FullManagement", 2)]
public class CalendarEvent
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(3000)]
    public string? Description { get; set; }

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public int? LinkedPostId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}