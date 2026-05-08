using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Financials.Models;

[HoaFeature("FullManagement", 2)]
public class LedgerEntry
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int OwnershipId { get; set; }
    public string EntryType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
}