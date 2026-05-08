using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Notifications.ViewModels;

namespace GearrOnes.HOA.Template.Features.Notifications.Services;

[HoaFeature("FullManagement", 2)]
public interface INotificationService
{
    Task CreateForUserAsync(string userId, string type, string title, string message, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationListItemViewModel>> GetForUserAsync(string userId, int take, CancellationToken cancellationToken);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken);
    Task MarkAsReadAsync(int notificationId, string userId, CancellationToken cancellationToken);
}