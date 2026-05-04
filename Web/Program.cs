using GearrOnes.HOA.Template.Config.Extensions;
using GearrOnes.HOA.Template.Core.Extensions;
using GearrOnes.HOA.Template.Features.Shared.Navigation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddPlanConfiguration(builder.Configuration);
builder.Services.AddHoaClientContext(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddScoped<INavigationService, NavigationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();