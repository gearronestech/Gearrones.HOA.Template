using GearrOnes.HOA.Template.Features.Accounts.Services;
using GearrOnes.HOA.Template.Features.Accounts.ViewModels;
using GearrOnes.HOA.Template.Features.Ownership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Features.Accounts.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IPropertyOwnershipService _ownershipService;

    public AccountController(IAccountService accountService, IPropertyOwnershipService ownershipService)
    {
        _accountService = accountService;
        _ownershipService = ownershipService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _accountService.RegisterAsync(model);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        var suggestion = await _ownershipService.FindBestMatchByEmailAsync(model.Email);
        if (suggestion is not null)
        {
            TempData["OwnershipMatch"] = $"Potential ownership match found: {suggestion.PersonName}. Board approval is required.";
        }

        return RedirectToAction(nameof(PendingApproval));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _accountService.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        if (!user.IsApproved)
        {
            return RedirectToAction(nameof(PendingApproval));
        }

        var result = await _accountService.LoginAsync(model);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult PendingApproval() => View();
}