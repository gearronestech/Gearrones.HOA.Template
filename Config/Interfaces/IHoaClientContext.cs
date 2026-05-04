namespace GearrOnes.HOA.Template.Config.Interfaces;

public interface IHoaClientContext
{
    string HoaName { get; }
    string LegalName { get; }
    string PortalTitle { get; }
    string MailingAddress { get; }
    string ContactEmail { get; }
    string SupportEmail { get; }
    string Phone { get; }
    string WebsiteUrl { get; }
    string LogoPath { get; }
}