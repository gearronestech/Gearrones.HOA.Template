using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Calendar.ViewModels;

namespace GearrOnes.HOA.Template.Features.Calendar.Services;

[HoaFeature("FullManagement", 2)]
public interface ICalendarEventService
{
    Task<CalendarMonthViewModel> GetMonthAsync(int year, int month, CancellationToken cancellationToken = default);
    Task<CalendarEventDetailViewModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CalendarEventUpsertViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task SaveAsync(CalendarEventUpsertViewModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}