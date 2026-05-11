using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Calendar.Services;
using GearrOnes.HOA.Template.Features.Calendar.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[Authorize]
[HoaFeature("FullManagement", 2, FeatureKey = FeatureKeys.Calendar)]
public class CalendarController : Controller
{
    private readonly ICalendarEventService _service;

    public CalendarController(ICalendarEventService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int? year, int? month, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var targetYear = year ?? now.Year;
        var targetMonth = month ?? now.Month;
        var vm = await _service.GetMonthAsync(targetYear, targetMonth, cancellationToken);
        return View(vm);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var vm = await _service.GetByIdAsync(id, cancellationToken);
        return vm is null ? NotFound() : View(vm);
    }

    [Authorize(Roles = "Admin,BoardMember")]
    [HttpGet]
    public async Task<IActionResult> Upsert(int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
        {
            return View(new CalendarEventUpsertViewModel
            {
                StartUtc = DateTime.UtcNow,
                EndUtc = DateTime.UtcNow.AddHours(1)
            });
        }

        var vm = await _service.GetForEditAsync(id.Value, cancellationToken);
        return vm is null ? NotFound() : View(vm);
    }

    [Authorize(Roles = "Admin,BoardMember")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(CalendarEventUpsertViewModel model, CancellationToken cancellationToken)
    {
        if (model.EndUtc <= model.StartUtc)
        {
            ModelState.AddModelError(nameof(model.EndUtc), "End date/time must be after start date/time.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _service.SaveAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,BoardMember")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}