using GearrOnes.HOA.Template.Features.Accounts.Models;
using GearrOnes.HOA.Template.Features.Accounts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Accounts.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsApproved = false,
            CreatedUtc = DateTime.UtcNow,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return result;
        }

        await _userManager.AddToRoleAsync(user, "Homeowner");
        await _emailSender.SendEmailAsync(user.Email!, "Confirm your account", "Please confirm your email.");
        return result;
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || !user.IsApproved)
        {
            return SignInResult.NotAllowed;
        }

        return await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
    }

    public Task<ApplicationUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

    public Task LogoutAsync() => _signInManager.SignOutAsync();

    public async Task<IReadOnlyList<ApplicationUser>> GetPendingUsersAsync()
    {
        return await _userManager.Users.Where(x => !x.IsApproved).OrderBy(x => x.CreatedUtc).ToListAsync();
    }

    public async Task<bool> ApproveUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return false;
        }

        user.IsApproved = true;
        user.ApprovedUtc = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        if (!await _userManager.IsInRoleAsync(user, "Homeowner"))
        {
            await _userManager.AddToRoleAsync(user, "Homeowner");
        }

        return true;
    }
}