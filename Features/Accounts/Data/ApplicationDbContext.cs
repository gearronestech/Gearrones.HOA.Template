using GearrOnes.HOA.Template.Features.Financials.Models;
using GearrOnes.HOA.Template.Features.Requests.Models;
using GearrOnes.HOA.Template.Features.Ownership.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Accounts.Data;

public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<RequestAttachment> RequestAttachments => Set<RequestAttachment>();
    public DbSet<PropertyOwnership> PropertyOwnerships => Set<PropertyOwnership>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<Payment> Payments => Set<Payment>();
}