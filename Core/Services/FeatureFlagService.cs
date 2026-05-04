using System;
using System.Collections.Generic;
using GearrOnes.HOA.Template.Core.Constants;
using GearrOnes.HOA.Template.Core.Interfaces;
using GearrOnes.HOA.Template.Core.Models;

namespace GearrOnes.HOA.Template.Core.Services;

public sealed class FeatureFlagService : IFeatureFlagService
{
    private readonly FeatureFlags _flags;
    private readonly IReadOnlyDictionary<string, Func<FeatureFlags, bool>> _featureMap;

    public FeatureFlagService(FeatureFlags flags)
    {
        _flags = flags;
        _featureMap = new Dictionary<string, Func<FeatureFlags, bool>>(StringComparer.OrdinalIgnoreCase)
        {
            [FeatureKeys.Dashboard] = f => f.Dashboard,
            [FeatureKeys.Documents] = f => f.Documents,
            [FeatureKeys.Requests] = f => f.Requests,
            [FeatureKeys.Accounts] = f => f.Accounts,
            [FeatureKeys.Board] = f => f.Board,
            [FeatureKeys.Financials] = f => f.Financials,
            [FeatureKeys.Community] = f => f.Community,
            [FeatureKeys.Violations] = f => f.Violations,
            [FeatureKeys.Voting] = f => f.Voting,
            [FeatureKeys.ExternalIntegrations] = f => f.ExternalIntegrations,
        };
    }

    public bool IsEnabled(string featureKey)
    {
        if (string.IsNullOrWhiteSpace(featureKey))
        {
            return false;
        }

        return _featureMap.TryGetValue(featureKey, out var accessor) && accessor(_flags);
    }

    public bool IsDisabled(string featureKey) => !IsEnabled(featureKey);

    public FeatureFlags GetAll() => _flags;
}