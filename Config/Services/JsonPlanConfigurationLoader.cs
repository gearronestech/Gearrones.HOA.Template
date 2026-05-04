using System.Text.Json;
using GearrOnes.HOA.Template.Config.Interfaces;
using GearrOnes.HOA.Template.Config.Models;

namespace GearrOnes.HOA.Template.Config.Services;

public sealed class JsonPlanConfigurationLoader : IPlanConfigurationLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PlanConfiguration Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Plan configuration path is required.", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Plan configuration file was not found at path '{path}'.", path);
        }

        try
        {
            var json = File.ReadAllText(path);
            var plan = JsonSerializer.Deserialize<PlanConfiguration>(json, SerializerOptions)
                ?? throw new InvalidOperationException($"Plan configuration file '{path}' is empty or invalid.");

            Validate(plan, path);
            return plan;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Plan configuration file '{path}' contains invalid JSON.", ex);
        }
    }

    private static void Validate(PlanConfiguration plan, string path)
    {
        if (string.IsNullOrWhiteSpace(plan.Bundle))
        {
            throw new InvalidOperationException($"Plan configuration file '{path}' must include a non-empty Bundle value.");
        }

        if (plan.Tier <= 0)
        {
            throw new InvalidOperationException($"Plan configuration file '{path}' must include a Tier value greater than 0.");
        }

        if (plan.FeatureFlags == null)
        {
            throw new InvalidOperationException($"Plan configuration file '{path}' must include FeatureFlags.");
        }
    }
}