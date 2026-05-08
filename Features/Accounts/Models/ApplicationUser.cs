using Microsoft.AspNetCore.Identity;

namespace GearrOnes.HOA.Template.Features.Accounts.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime? ApprovedUtc { get; set; }
    public int? OwnershipPersonId { get; set; }
    public DateTime? OwnershipLinkApprovedUtc { get; set; }
}