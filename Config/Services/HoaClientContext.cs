using GearrOnes.HOA.Template.Config.Interfaces;
using GearrOnes.HOA.Template.Config.Models;
using Microsoft.Extensions.Options;

namespace GearrOnes.HOA.Template.Config.Services;

public class HoaClientContext : IHoaClientContext
{
    public HoaClientContext(IOptions<HoaClientOptions> options)
    {
        var value = options.Value;
        HoaName = value.HoaName ?? string.Empty;
        LegalName = value.LegalName ?? string.Empty;
        PortalTitle = value.PortalTitle ?? string.Empty;
        MailingAddress = value.MailingAddress ?? string.Empty;
        ContactEmail = value.ContactEmail ?? string.Empty;
        SupportEmail = value.SupportEmail ?? string.Empty;
        Phone = value.Phone ?? string.Empty;
        WebsiteUrl = value.WebsiteUrl ?? string.Empty;
        LogoPath = value.LogoPath ?? string.Empty;
    }

    public string HoaName { get; }
    public string LegalName { get; }
    public string PortalTitle { get; }
    public string MailingAddress { get; }
    public string ContactEmail { get; }
    public string SupportEmail { get; }
    public string Phone { get; }
    public string WebsiteUrl { get; }
    public string LogoPath { get; }
}