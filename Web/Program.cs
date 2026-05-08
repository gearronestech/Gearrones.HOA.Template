using GearrOnes.HOA.Template.Config.Extensions;
using GearrOnes.HOA.Template.Core.Extensions;
using GearrOnes.HOA.Template.Features.Shared.Navigation;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Accounts.Models;
using GearrOnes.HOA.Template.Features.Accounts.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GearrOnes.HOA.Template.Features.Requests.Services;
using GearrOnes.HOA.Template.Features.Ownership.Services;
using GearrOnes.HOA.Template.Features.Financials.Services;
using GearrOnes.HOA.Template.Features.Notifications.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddPlanConfiguration(builder.Configuration);
builder.Services.AddHoaClientContext(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();
builder.Services.AddSingleton<IRequestMessagingService, InMemoryRequestMessagingService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPropertyOwnershipService, PropertyOwnershipService>();
builder.Services.AddScoped<IFinancialLedgerService, FinancialLedgerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

await IdentitySeeder.SeedRolesAsync(app.Services);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();