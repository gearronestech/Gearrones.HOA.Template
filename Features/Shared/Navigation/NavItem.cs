namespace GearrOnes.HOA.Template.Features.Shared.Navigation;

public sealed class NavItem
{
    public required string Title { get; init; }
    public required string Controller { get; init; }
    public required string Action { get; init; }
    public required string FeatureKey { get; init; }
    public string? RequiredRole { get; init; }
    public string? IconClass { get; init; }
}