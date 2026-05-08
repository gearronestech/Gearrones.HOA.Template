using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Financials.Models;
using GearrOnes.HOA.Template.Features.Financials.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Financials.Services;

[HoaFeature("FullManagement", 2)]
public class FinancialLedgerService : IFinancialLedgerService
{
    private readonly ApplicationDbContext _db;
    public FinancialLedgerService(ApplicationDbContext db) => _db = db;

    public async Task<FinancialLedgerViewModel?> GetLedgerForPersonAsync(int personId)
    {
        var ownership = await _db.PropertyOwnerships.Where(x => x.PersonId == personId).OrderByDescending(x => x.StartDate).FirstOrDefaultAsync();
        if (ownership is null) return null;

        var property = await _db.Properties.FirstAsync(x => x.Id == ownership.PropertyId);
        return await BuildLedgerAsync(ownership.Id, $"{property.AddressLine1}", await _db.Persons.Where(x => x.Id == ownership.PersonId).Select(x => x.FullName).FirstAsync());
    }

    public async Task<IReadOnlyList<(int OwnershipId, string Display)>> GetOwnershipOptionsAsync() =>
        await (from o in _db.PropertyOwnerships
               join p in _db.Properties on o.PropertyId equals p.Id
               join person in _db.Persons on o.PersonId equals person.Id
               where o.EndDate == null
               orderby p.AddressLine1
               select new ValueTuple<int, string>(o.Id, $"{p.AddressLine1} - {person.FullName}"))
               .ToListAsync();

    public async Task AddManualChargeAsync(AddLedgerEntryViewModel model)
    {
        _db.LedgerEntries.Add(new LedgerEntry { PropertyId = model.PropertyId, OwnershipId = model.OwnershipId, EntryType = model.EntryType, Amount = model.Amount, DueDate = model.DueDate.Date, Description = model.Description, CreatedUtc = DateTime.UtcNow });
        await _db.SaveChangesAsync();
    }

    public async Task AddManualPaymentAsync(AddPaymentViewModel model)
    {
        _db.Payments.Add(new Payment { PropertyId = model.PropertyId, OwnershipId = model.OwnershipId, Amount = model.Amount, PaymentMethod = model.PaymentMethod, ReferenceNumber = model.ReferenceNumber, CreatedUtc = DateTime.UtcNow });
        await _db.SaveChangesAsync();
    }

    public async Task<FinancialLedgerViewModel?> GetLedgerByOwnershipAsync(int ownershipId)
    {
        var ownership = await _db.PropertyOwnerships.FirstOrDefaultAsync(x => x.Id == ownershipId);
        if (ownership is null) return null;
        var property = await _db.Properties.FirstAsync(x => x.Id == ownership.PropertyId);
        var owner = await _db.Persons.FirstAsync(x => x.Id == ownership.PersonId);
        return await BuildLedgerAsync(ownershipId, property.AddressLine1, owner.FullName);
    }

    private async Task<FinancialLedgerViewModel> BuildLedgerAsync(int ownershipId, string propertyAddress, string accountHolder)
    {
        var charges = await _db.LedgerEntries.Where(x => x.OwnershipId == ownershipId).Select(x => new { DateUtc = x.CreatedUtc, x.Description, Amount = x.Amount, x.EntryType }).ToListAsync();
        var payments = await _db.Payments.Where(x => x.OwnershipId == ownershipId).Select(x => new { DateUtc = x.CreatedUtc, Description = $"Payment ({x.PaymentMethod}) #{x.ReferenceNumber}", Amount = x.Amount }).ToListAsync();
        var tx = charges.Select(x => (x.DateUtc, Type: x.EntryType, x.Description, Charge: x.Amount, Payment: 0m))
            .Concat(payments.Select(x => (x.DateUtc, Type: "Payment", x.Description, Charge: 0m, Payment: x.Amount)))
            .OrderBy(x => x.DateUtc)
            .ToList();

        decimal running = 0;
        var rows = new List<FinancialTransactionViewModel>();
        foreach (var row in tx)
        {
            running += row.Charge - row.Payment;
            rows.Add(new FinancialTransactionViewModel { DateUtc = row.DateUtc, Type = row.Type, Description = row.Description, ChargeAmount = row.Charge, PaymentAmount = row.Payment, BalanceAfter = running });
        }

        return new FinancialLedgerViewModel { AccountHolder = accountHolder, PropertyAddress = propertyAddress, RunningBalance = running, TotalCharges = charges.Sum(x => x.Amount), TotalPayments = payments.Sum(x => x.Amount), Transactions = rows };
    }
}