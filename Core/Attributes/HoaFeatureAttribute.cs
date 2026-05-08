using System;

namespace GearrOnes.HOA.Template.Core.Attributes;

/// <summary>
/// Tags HOA portal code with bundle/tier/feature metadata.
/// Intended for controllers, services, models, interfaces, and major methods.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Interface |
    AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true)]
public sealed class HoaFeatureAttribute : Attribute
{
    public HoaFeatureAttribute(string bundle, int tier)
    {
        Bundle = bundle;
        Tier = tier;
    }

    public string Bundle { get; }

    public int Tier { get; }

    public string? FeatureKey { get; init; }

    public string? Description { get; init; }
}