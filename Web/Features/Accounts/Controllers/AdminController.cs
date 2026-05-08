using GearrOnes.HOA.Template.Features.Accounts.Services;
using GearrOnes.HOA.Template.Features.Ownership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Features.Accounts.Controllers;

[Authorize(Roles = "Admin,BoardMember")]
public class AdminController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IPropertyOwnershipService _ownershipService;

    public AdminController(IAccountService accountService, IPropertyOwnershipService ownershipService)
    {
        _accountService = accountService;
        _ownershipService = ownershipService;
    }

    [HttpGet]
    public async Task<IActionResult> PendingUsers()
    {
        var pendingUsers = await _accountService.GetPendingUsersAsync();
        return View(pendingUsers);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveUser(string id)
    {
        var approved = await _accountService.ApproveUserAsync(id);
        if (!approved) return NotFound();

        return RedirectToAction(nameof(PendingUsers));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BoardMember,Board")]
    public async Task<IActionResult> ApproveOwnershipLink(string userId, int personId)
    {
        await _ownershipService.ApproveUserOwnershipLinkAsync(userId, personId);
        return RedirectToAction(nameof(PendingUsers));
    }
}