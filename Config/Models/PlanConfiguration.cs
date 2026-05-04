using GearrOnes.HOA.Template.Core.Models;

namespace GearrOnes.HOA.Template.Config.Models;

public class PlanConfiguration
{
    public string Bundle { get; set; } = string.Empty;
    public int Tier { get; set; }
    public string PlanDisplayName { get; set; } = string.Empty;
    public FeatureFlags FeatureFlags { get; set; } = new();
}