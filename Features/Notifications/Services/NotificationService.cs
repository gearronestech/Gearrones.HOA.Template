using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Accounts.Data;
using GearrOnes.HOA.Template.Features.Notifications.Models;
using GearrOnes.HOA.Template.Features.Notifications.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GearrOnes.HOA.Template.Features.Notifications.Services;

[HoaFeature("FullManagement", 2)]
public sealed class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateForUserAsync(string userId, string type, string title, string message, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedUtc = DateTime.UtcNow,
        };

        _dbContext.Add(notification);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Email placeholder only.
        _ = $"EMAIL_PLACEHOLDER:{userId}:{title}";
    }

    public async Task<IReadOnlyList<NotificationListItemViewModel>> GetForUserAsync(string userId, int take, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Notification>()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedUtc)
            .Take(take)
            .Select(n => new NotificationListItemViewModel
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedUtc = n.CreatedUtc,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken) =>
        _dbContext.Set<Notification>().CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

    public async Task MarkAsReadAsync(int notificationId, string userId, CancellationToken cancellationToken)
    {
        var notification = await _dbContext.Set<Notification>()
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId, cancellationToken);

        if (notification is null || notification.IsRead)
        {
            return;
        }

        notification.IsRead = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}