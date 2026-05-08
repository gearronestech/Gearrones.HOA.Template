using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Financials.Models;

[HoaFeature("FullManagement", 2)]
public class Payment
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int OwnershipId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
}