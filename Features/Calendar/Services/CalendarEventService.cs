using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Calendar.Models;
using GearrOnes.HOA.Template.Features.Calendar.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Calendar.Services;

[HoaFeature("FullManagement", 2)]
public class CalendarEventService : ICalendarEventService
{
    private readonly ApplicationDbContext _dbContext;

    public CalendarEventService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CalendarMonthViewModel> GetMonthAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var first = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var gridStart = first.AddDays(-(int)first.DayOfWeek);
        var gridEnd = gridStart.AddDays(42);

        var events = await _dbContext.CalendarEvents
            .AsNoTracking()
            .Where(e => e.StartUtc < gridEnd && e.EndUtc >= gridStart)
            .OrderBy(e => e.StartUtc)
            .ToListAsync(cancellationToken);

        var days = Enumerable.Range(0, 42)
            .Select(offset => gridStart.AddDays(offset))
            .Select(day => new CalendarDayViewModel
            {
                Date = day,
                IsCurrentMonth = day.Month == month,
                Events = events
                    .Where(e => e.StartUtc.Date <= day.Date && e.EndUtc.Date >= day.Date)
                    .Select(e => new CalendarListItemViewModel
                    {
                        Id = e.Id,
                        Title = e.Title,
                        StartUtc = e.StartUtc,
                        EndUtc = e.EndUtc
                    }).ToList()
            }).ToList();

        return new CalendarMonthViewModel
        {
            Year = year,
            Month = month,
            MonthLabel = first.ToString("MMMM yyyy"),
            Days = days
        };
    }

    public async Task<CalendarEventDetailViewModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.CalendarEvents
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new CalendarEventDetailViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartUtc = e.StartUtc,
                EndUtc = e.EndUtc,
                LinkedPostId = e.LinkedPostId,
                CreatedUtc = e.CreatedUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<CalendarEventUpsertViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.CalendarEvents
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new CalendarEventUpsertViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartUtc = e.StartUtc,
                EndUtc = e.EndUtc,
                LinkedPostId = e.LinkedPostId
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task SaveAsync(CalendarEventUpsertViewModel model, CancellationToken cancellationToken = default)
    {
        CalendarEvent entity;
        if (model.Id.HasValue)
        {
            entity = await _dbContext.CalendarEvents.FirstAsync(e => e.Id == model.Id.Value, cancellationToken);
        }
        else
        {
            entity = new CalendarEvent();
            _dbContext.CalendarEvents.Add(entity);
        }

        entity.Title = model.Title.Trim();
        entity.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        entity.StartUtc = DateTime.SpecifyKind(model.StartUtc, DateTimeKind.Utc);
        entity.EndUtc = DateTime.SpecifyKind(model.EndUtc, DateTimeKind.Utc);
        entity.LinkedPostId = model.LinkedPostId;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.CalendarEvents.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.CalendarEvents.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}