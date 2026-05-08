using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Ownership.Models;
using GearrOnes.HOA.Template.Features.Ownership.ViewModels;

namespace GearrOnes.HOA.Template.Features.Ownership.Services;

[HoaFeature("FullManagement", 2)]
public interface IPropertyOwnershipService
{
    Task<IReadOnlyList<OwnershipHistoryItemViewModel>> GetOwnershipHistoryAsync();
    Task<UpsertPropertyOwnershipViewModel?> GetByIdAsync(int id);
    Task SaveAsync(UpsertPropertyOwnershipViewModel model);
    Task<UpsertContext> GetContextAsync();
    Task<OwnershipLinkSuggestion?> FindBestMatchByEmailAsync(string email);
    Task ApproveUserOwnershipLinkAsync(string userId, int personId);
}

[HoaFeature("FullManagement", 2)]
public record UpsertContext(IReadOnlyList<Property> Properties, IReadOnlyList<Person> People);

[HoaFeature("FullManagement", 2)]
public record OwnershipLinkSuggestion(int PersonId, string PersonName, string Reason);