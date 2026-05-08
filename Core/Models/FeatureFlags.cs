namespace GearrOnes.HOA.Template.Core.Models;

/// <summary>
/// Feature flag values for the application.
/// </summary>
public class FeatureFlags
{
    public bool Dashboard { get; set; }
    public bool Documents { get; set; }
    public bool Requests { get; set; }
    public bool RequestMessaging { get; set; }
    public bool RequestAttachments { get; set; }
    public bool Accounts { get; set; }
    public bool Board { get; set; }
    public bool Financials { get; set; }
    public bool FinancialLedger { get; set; }
    public bool Community { get; set; }
    public bool Violations { get; set; }
    public bool Voting { get; set; }
    public bool ExternalIntegrations { get; set; }
    public bool OwnershipTracking { get; set; }
    public bool Notifications { get; set; }
}