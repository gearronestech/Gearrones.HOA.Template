using GearrOnes.HOA.Template.Core.Models;

namespace GearrOnes.HOA.Template.Core.Interfaces;

public interface IFeatureFlagService
{
    bool IsEnabled(string featureKey);
    bool IsDisabled(string featureKey);
    FeatureFlags GetAll();
}