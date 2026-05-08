using System.Security.Claims;
using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Notifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[HoaFeature("FullManagement", 1, FeatureKey = FeatureKeys.Documents)]
public class DocumentsController : Controller
{
    private readonly INotificationService _notificationService;

    public DocumentsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> UploadPlaceholder(CancellationToken cancellationToken)
    {
        await _notificationService.CreateForUserAsync(GetUserId(), "DocumentUpload", "Document uploaded", "A new community document was uploaded.", cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "homeowner-1";
}