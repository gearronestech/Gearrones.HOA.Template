using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Ownership.Models;

[HoaFeature("FullManagement", 2)]
public class Property
{
    public int Id { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
}