using System.ComponentModel.DataAnnotations;
using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Calendar.ViewModels;

[HoaFeature("FullManagement", 2)]
public class CalendarEventUpsertViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(3000)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartUtc { get; set; }

    [Required]
    public DateTime EndUtc { get; set; }

    public int? LinkedPostId { get; set; }
}