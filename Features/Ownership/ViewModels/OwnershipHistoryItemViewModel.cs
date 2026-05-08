using System.ComponentModel.DataAnnotations;
using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Ownership.ViewModels;

[HoaFeature("FullManagement", 2)]
public class OwnershipHistoryItemViewModel
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyDisplay { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public string PersonDisplay { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ClosingReference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

[HoaFeature("FullManagement", 2)]
public class UpsertPropertyOwnershipViewModel
{
    public int? Id { get; set; }

    [Required]
    public int PropertyId { get; set; }

    [Required]
    public int PersonId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    public string ClosingReference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}