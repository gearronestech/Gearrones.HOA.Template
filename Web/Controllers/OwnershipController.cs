using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Ownership.Services;
using GearrOnes.HOA.Template.Features.Ownership.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GearrOnes.HOA.Template.Web.Controllers;

[Authorize(Roles = "Admin,BoardMember,Board")]
[HoaFeature("FullManagement", 2, FeatureKey = FeatureKeys.OwnershipTracking)]
public class OwnershipController : Controller
{
    private readonly IPropertyOwnershipService _service;
    public OwnershipController(IPropertyOwnershipService service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.GetOwnershipHistoryAsync());

    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        var model = id.HasValue ? await _service.GetByIdAsync(id.Value) ?? new UpsertPropertyOwnershipViewModel() : new UpsertPropertyOwnershipViewModel { StartDate = DateTime.UtcNow.Date };
        await BindLookupsAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(UpsertPropertyOwnershipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await BindLookupsAsync();
            return View(model);
        }

        await _service.SaveAsync(model);
        return RedirectToAction(nameof(Index));
    }

    private async Task BindLookupsAsync()
    {
        var ctx = await _service.GetContextAsync();
        ViewBag.Properties = new SelectList(ctx.Properties, "Id", "AddressLine1");
        ViewBag.People = new SelectList(ctx.People, "Id", "FullName");
    }
}