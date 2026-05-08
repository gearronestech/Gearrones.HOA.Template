using GearrOnes.HOA.Template.Features.Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Features.Accounts.Controllers;

[Authorize(Roles = "Admin,BoardMember")]
public class AdminController : Controller
{
    private readonly IAccountService _accountService;

    public AdminController(IAccountService accountService)
    {
        _accountService = accountService;
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
}