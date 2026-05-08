using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Financials.Services;
using GearrOnes.HOA.Template.Features.Financials.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Web.Controllers;

[Authorize]
[HoaFeature("FullManagement", 2, FeatureKey = FeatureKeys.FinancialLedger)]
public class FinancialsController : Controller
{
    private readonly IFinancialLedgerService _service;
    private readonly ApplicationDbContext _db;
    public FinancialsController(IFinancialLedgerService service, ApplicationDbContext db) { _service = service; _db = db; }

    public async Task<IActionResult> Index()
    {
        var email = User.Identity?.Name;
        var personId = await _db.Persons.Where(x => x.Email == email).Select(x => (int?)x.Id).FirstOrDefaultAsync();
        if (personId is null) return View(new FinancialLedgerViewModel());
        var ledger = await _service.GetLedgerForPersonAsync(personId.Value);
        return View(ledger ?? new FinancialLedgerViewModel());
    }

    [Authorize(Roles = "Admin,BoardMember,Board")]
    [HttpGet]
    public async Task<IActionResult> Manage()
    {
        await BindOwnerships();
        return View();
    }

    [Authorize(Roles = "Admin,BoardMember,Board")]
    [HttpPost]
    public async Task<IActionResult> AddCharge(AddLedgerEntryViewModel model)
    {
        await _service.AddManualChargeAsync(model);
        return RedirectToAction(nameof(Manage));
    }

    [Authorize(Roles = "Admin,BoardMember,Board")]
    [HttpPost]
    public async Task<IActionResult> AddPayment(AddPaymentViewModel model)
    {
        await _service.AddManualPaymentAsync(model);
        return RedirectToAction(nameof(Manage));
    }

    private async Task BindOwnerships()
    {
        var options = await _service.GetOwnershipOptionsAsync();
        ViewBag.Ownerships = new SelectList(options.Select(x => new { Id = x.OwnershipId, Display = x.Display }), "Id", "Display");
    }
}