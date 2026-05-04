using GearrOnes.HOA.Template.Core.Models;

namespace GearrOnes.HOA.Template.Config.Interfaces;

public interface IPlanContext
{
    string Bundle { get; }
    int Tier { get; }
    string PlanDisplayName { get; }
    FeatureFlags FeatureFlags { get; }
}