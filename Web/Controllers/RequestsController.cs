using System.Security.Claims;
using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Notifications.Services;
using GearrOnes.HOA.Template.Features.Requests.Services;
using GearrOnes.HOA.Template.Features.Requests.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[HoaFeature("FullManagement", 1, FeatureKey = FeatureKeys.Requests)]
public class RequestsController : Controller
{
    private readonly IRequestMessagingService _requestMessagingService;
    private readonly INotificationService _notificationService;

    public RequestsController(IRequestMessagingService requestMessagingService, INotificationService notificationService)
    {
        _requestMessagingService = requestMessagingService;
        _notificationService = notificationService;
    }

    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var requests = await _requestMessagingService.GetVisibleRequestsAsync(GetUserId(), GetRole(), cancellationToken);
        return View(requests);
    }

    [HttpGet]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var model = await _requestMessagingService.GetRequestDetailsAsync(id, GetUserId(), GetRole(), cancellationToken);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> AddMessage(CreateRequestMessageInputModel input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Message))
        {
            ModelState.AddModelError(nameof(input.Message), "Message is required.");
            return RedirectToAction(nameof(Details), new { id = input.RequestId });
        }

        await _requestMessagingService.AddMessageAsync(input, GetUserId(), GetRole(), cancellationToken);
        await _notificationService.CreateForUserAsync(GetUserId(), "RequestUpdate", "Request updated", $"A new message was added to request #{input.RequestId}.", cancellationToken);
        return RedirectToAction(nameof(Details), new { id = input.RequestId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> UploadAttachment(UploadRequestAttachmentInputModel input, CancellationToken cancellationToken)
    {
        if (input.File is null)
        {
            return RedirectToAction(nameof(Details), new { id = input.RequestId });
        }

        await _requestMessagingService.AddAttachmentAsync(input, GetUserId(), GetRole(), cancellationToken);
        await _notificationService.CreateForUserAsync(GetUserId(), "DocumentUpload", "Document uploaded", $"A document was uploaded to request #{input.RequestId}.", cancellationToken);
        return RedirectToAction(nameof(Details), new { id = input.RequestId });
    }

    [HttpGet]
    [HoaFeature("FullManagement", 2)]
    public async Task<IActionResult> DownloadAttachment(int id, CancellationToken cancellationToken)
    {
        var file = await _requestMessagingService.GetAttachmentAsync(id, GetUserId(), GetRole(), cancellationToken);
        return file is null ? NotFound() : File(file.ContentStream, file.ContentType, file.OriginalFileName);
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "homeowner-1";
    private string GetRole() => User.FindFirstValue(ClaimTypes.Role) ?? "Homeowner";
}