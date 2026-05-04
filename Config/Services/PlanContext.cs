using GearrOnes.HOA.Template.Config.Interfaces;
using GearrOnes.HOA.Template.Config.Models;
using GearrOnes.HOA.Template.Core.Models;

namespace GearrOnes.HOA.Template.Config.Services;

public sealed class PlanContext : IPlanContext
{
    public PlanContext(PlanConfiguration planConfiguration)
    {
        Bundle = planConfiguration.Bundle;
        Tier = planConfiguration.Tier;
        PlanDisplayName = planConfiguration.PlanDisplayName;
        FeatureFlags = planConfiguration.FeatureFlags
            ?? throw new InvalidOperationException("Plan configuration must include FeatureFlags.");
    }

    public string Bundle { get; }
    public int Tier { get; }
    public string PlanDisplayName { get; }
    public FeatureFlags FeatureFlags { get; }
}