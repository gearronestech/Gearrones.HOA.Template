using System.Security.Claims;
using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Notifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[HoaFeature("FullManagement", 1, FeatureKey = FeatureKeys.Dashboard)]
public class DashboardController : Controller
{
    private readonly INotificationService _notificationService;

    public DashboardController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> PublishAnnouncementPlaceholder(CancellationToken cancellationToken)
    {
        await _notificationService.CreateForUserAsync(GetUserId(), "Announcement", "New announcement", "A new board announcement has been posted.", cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "homeowner-1";
}