using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Financials.ViewModels;

[HoaFeature("FullManagement", 2)]
public class FinancialLedgerViewModel
{
    public string AccountHolder { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public decimal RunningBalance { get; set; }
    public decimal TotalCharges { get; set; }
    public decimal TotalPayments { get; set; }
    public IReadOnlyList<FinancialTransactionViewModel> Transactions { get; set; } = [];
}

[HoaFeature("FullManagement", 2)]
public class FinancialTransactionViewModel
{
    public DateTime DateUtc { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal ChargeAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal BalanceAfter { get; set; }
}

[HoaFeature("FullManagement", 2)]
public class AddLedgerEntryViewModel
{
    public int PropertyId { get; set; }
    public int OwnershipId { get; set; }
    public string EntryType { get; set; } = "ManualCharge";
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UtcNow.Date;
    public string Description { get; set; } = string.Empty;
}

[HoaFeature("FullManagement", 2)]
public class AddPaymentViewModel
{
    public int PropertyId { get; set; }
    public int OwnershipId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "Manual";
    public string ReferenceNumber { get; set; } = string.Empty;
}