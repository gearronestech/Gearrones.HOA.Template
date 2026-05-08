using System.Security.Claims;
using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Notifications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[Authorize]
[HoaFeature("FullManagement", 2, FeatureKey = FeatureKeys.Notifications)]
public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _notificationService.GetForUserAsync(GetUserId(), 100, cancellationToken);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        await _notificationService.MarkAsReadAsync(id, GetUserId(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "homeowner-1";
}