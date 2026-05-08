using GearrOnes.HOA.Template.Features.Requests.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Accounts.Data;

public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<RequestAttachment> RequestAttachments => Set<RequestAttachment>();
}