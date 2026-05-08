using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Ownership.Models;

[HoaFeature("FullManagement", 2)]
public class PropertyOwnership
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int PersonId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ClosingReference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
}