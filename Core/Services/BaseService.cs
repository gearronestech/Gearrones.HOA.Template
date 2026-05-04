using GearrOnes.HOA.Template.Core.Exceptions;
using GearrOnes.HOA.Template.Core.Interfaces;

namespace GearrOnes.HOA.Template.Core.Services;

public abstract class BaseService
{
    private readonly IFeatureFlagService _featureFlagService;

    protected BaseService(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }

    protected void EnsureFeatureEnabled(string featureKey)
    {
        if (_featureFlagService.IsDisabled(featureKey))
        {
            throw new FeatureNotEnabledException(featureKey);
        }
    }
}