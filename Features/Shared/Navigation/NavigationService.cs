using System.Security.Claims;
using GearrOnes.HOA.Template.Config.Interfaces;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Core.Interfaces;

namespace GearrOnes.HOA.Template.Features.Shared.Navigation;

public sealed class NavigationService : INavigationService
{
    private readonly IFeatureFlagService _featureFlagService;
    private readonly IPlanContext _planContext;

    public NavigationService(IFeatureFlagService featureFlagService, IPlanContext planContext)
    {
        _featureFlagService = featureFlagService;
        _planContext = planContext;
    }

    public IEnumerable<NavItem> GetMainNavigation(ClaimsPrincipal user)
    {
        var items = GetNavigationByRole(user);

        return items.Where(item => _featureFlagService.IsEnabled(item.FeatureKey));
    }

    private static IEnumerable<NavItem> GetHomeownerNavigation() =>
    [
        new NavItem { Title = "Dashboard", Controller = "Dashboard", Action = "Index", FeatureKey = FeatureKeys.Dashboard, IconClass = "bi bi-speedometer2" },
        new NavItem { Title = "Documents", Controller = "Documents", Action = "Index", FeatureKey = FeatureKeys.Documents, IconClass = "bi bi-file-earmark-text" },
        new NavItem { Title = "Requests", Controller = "Requests", Action = "Index", FeatureKey = FeatureKeys.Requests, IconClass = "bi bi-chat-left-text" },
        new NavItem { Title = "Financial Ledger", Controller = "Financials", Action = "Index", FeatureKey = FeatureKeys.FinancialLedger, IconClass = "bi bi-receipt" },
        new NavItem { Title = "Notifications", Controller = "Notifications", Action = "Index", FeatureKey = FeatureKeys.Notifications, IconClass = "bi bi-bell" },
        new NavItem { Title = "Calendar", Controller = "Calendar", Action = "Index", FeatureKey = FeatureKeys.Calendar, IconClass = "bi bi-calendar3" },
    ];

    private static IEnumerable<NavItem> GetBoardNavigation() =>
    [
        new NavItem { Title = "Board Dashboard", Controller = "Dashboard", Action = "Index", FeatureKey = FeatureKeys.Dashboard, RequiredRole = "Board", IconClass = "bi bi-speedometer2" },
        new NavItem { Title = "Documents", Controller = "Documents", Action = "Index", FeatureKey = FeatureKeys.Documents, RequiredRole = "Board", IconClass = "bi bi-file-earmark-text" },
        new NavItem { Title = "Requests", Controller = "Requests", Action = "Index", FeatureKey = FeatureKeys.Requests, RequiredRole = "Board", IconClass = "bi bi-chat-left-text" },
        new NavItem { Title = "Account Approvals", Controller = "Home", Action = "Index", FeatureKey = FeatureKeys.Accounts, RequiredRole = "Board", IconClass = "bi bi-person-check" },
        new NavItem { Title = "Board Members", Controller = "Home", Action = "Index", FeatureKey = FeatureKeys.Board, RequiredRole = "Board", IconClass = "bi bi-people" },
        new NavItem { Title = "Ownership History", Controller = "Ownership", Action = "Index", FeatureKey = FeatureKeys.OwnershipTracking, RequiredRole = "Board", IconClass = "bi bi-house-check" },
        new NavItem { Title = "Financial Management", Controller = "Financials", Action = "Manage", FeatureKey = FeatureKeys.FinancialLedger, RequiredRole = "Board", IconClass = "bi bi-cash-stack" },
        new NavItem { Title = "Notifications", Controller = "Notifications", Action = "Index", FeatureKey = FeatureKeys.Notifications, RequiredRole = "Board", IconClass = "bi bi-bell" },
        new NavItem { Title = "Calendar", Controller = "Calendar", Action = "Index", FeatureKey = FeatureKeys.Calendar, RequiredRole = "Board", IconClass = "bi bi-calendar3" },
    ];

    private static IEnumerable<NavItem> GetAdminNavigation() =>
    [
        new NavItem { Title = "Admin Panel", Controller = "Home", Action = "Index", FeatureKey = FeatureKeys.Board, RequiredRole = "Admin", IconClass = "bi bi-shield-lock" },
        new NavItem { Title = "Users", Controller = "Home", Action = "Index", FeatureKey = FeatureKeys.Accounts, RequiredRole = "Admin", IconClass = "bi bi-people-fill" },
        new NavItem { Title = "Roles", Controller = "Home", Action = "Index", FeatureKey = FeatureKeys.Accounts, RequiredRole = "Admin", IconClass = "bi bi-person-badge" },
    ];

    private IEnumerable<NavItem> GetNavigationByRole(ClaimsPrincipal user)
    {
        // TODO: replace with finalized role mapping once auth/identity integration is complete.
        var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
        var isAdmin = isAuthenticated && user.IsInRole("Admin");
        var isBoard = isAuthenticated && (user.IsInRole("Board") || user.IsInRole("Administrator"));

        if (isAdmin)
        {
            return GetBoardNavigation().Concat(GetAdminNavigation());
        }

        if (isBoard)
        {
            return GetBoardNavigation();
        }

        _ = _planContext.PlanDisplayName;
        return GetHomeownerNavigation();
    }
}