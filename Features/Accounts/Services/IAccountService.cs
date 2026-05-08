using GearrOnes.HOA.Template.Features.Accounts.Models;
using GearrOnes.HOA.Template.Features.Accounts.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GearrOnes.HOA.Template.Features.Accounts.Services;

public interface IAccountService
{
    Task<IdentityResult> RegisterAsync(RegisterViewModel model);
    Task<SignInResult> LoginAsync(LoginViewModel model);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task LogoutAsync();
    Task<IReadOnlyList<ApplicationUser>> GetPendingUsersAsync();
    Task<bool> ApproveUserAsync(string id);
}