using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Financials.ViewModels;

namespace GearrOnes.HOA.Template.Features.Financials.Services;

[HoaFeature("FullManagement", 2)]
public interface IFinancialLedgerService
{
    Task<FinancialLedgerViewModel?> GetLedgerForPersonAsync(int personId);
    Task<IReadOnlyList<(int OwnershipId, string Display)>> GetOwnershipOptionsAsync();
    Task AddManualChargeAsync(AddLedgerEntryViewModel model);
    Task AddManualPaymentAsync(AddPaymentViewModel model);
}