using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Ownership.Models;
using GearrOnes.HOA.Template.Features.Ownership.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Ownership.Services;

[HoaFeature("FullManagement", 2)]
public class PropertyOwnershipService : IPropertyOwnershipService
{
    private readonly ApplicationDbContext _db;

    public PropertyOwnershipService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<OwnershipHistoryItemViewModel>> GetOwnershipHistoryAsync()
    {
        var query = from o in _db.PropertyOwnerships
                    join p in _db.Properties on o.PropertyId equals p.Id
                    join person in _db.Persons on o.PersonId equals person.Id
                    orderby p.AddressLine1, o.StartDate descending
                    select new OwnershipHistoryItemViewModel
                    {
                        Id = o.Id,
                        PropertyId = o.PropertyId,
                        PropertyDisplay = p.AddressLine1,
                        PersonId = o.PersonId,
                        PersonDisplay = person.FullName,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        ClosingReference = o.ClosingReference,
                        Notes = o.Notes
                    };

        return await query.ToListAsync();
    }

    public async Task<UpsertPropertyOwnershipViewModel?> GetByIdAsync(int id) =>
        await _db.PropertyOwnerships.Where(x => x.Id == id)
            .Select(x => new UpsertPropertyOwnershipViewModel
            {
                Id = x.Id,
                PropertyId = x.PropertyId,
                PersonId = x.PersonId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ClosingReference = x.ClosingReference,
                Notes = x.Notes
            }).FirstOrDefaultAsync();

    public async Task SaveAsync(UpsertPropertyOwnershipViewModel model)
    {
        PropertyOwnership entity;
        if (model.Id.HasValue)
        {
            entity = await _db.PropertyOwnerships.FirstAsync(x => x.Id == model.Id.Value);
        }
        else
        {
            entity = new PropertyOwnership { CreatedUtc = DateTime.UtcNow };
            _db.PropertyOwnerships.Add(entity);
        }

        entity.PropertyId = model.PropertyId;
        entity.PersonId = model.PersonId;
        entity.StartDate = model.StartDate.Date;
        entity.EndDate = model.EndDate?.Date;
        entity.ClosingReference = model.ClosingReference;
        entity.Notes = model.Notes;

        await _db.SaveChangesAsync();
    }

    public async Task<UpsertContext> GetContextAsync() =>
        new(await _db.Properties.OrderBy(x => x.AddressLine1).ToListAsync(), await _db.Persons.OrderBy(x => x.FullName).ToListAsync());

    public async Task<OwnershipLinkSuggestion?> FindBestMatchByEmailAsync(string email)
    {
        var person = await _db.Persons.FirstOrDefaultAsync(x => x.Email == email);
        return person is null ? null : new OwnershipLinkSuggestion(person.Id, person.FullName, "Matched by homeowner email.");
    }

    public async Task ApproveUserOwnershipLinkAsync(string userId, int personId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return;
        user.OwnershipPersonId = personId;
        user.OwnershipLinkApprovedUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}