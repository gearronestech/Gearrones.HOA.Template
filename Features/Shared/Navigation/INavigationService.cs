using System.Security.Claims;

namespace GearrOnes.HOA.Template.Features.Shared.Navigation;

public interface INavigationService
{
    IEnumerable<NavItem> GetMainNavigation(ClaimsPrincipal user);
}