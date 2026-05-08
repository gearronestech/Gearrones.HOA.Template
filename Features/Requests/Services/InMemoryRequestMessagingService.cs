using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Features.Requests.Models;
using GearrOnes.HOA.Template.Features.Requests.ViewModels;

namespace GearrOnes.HOA.Template.Features.Requests.Services;

[HoaFeature("FullManagement", 2)]
public sealed class InMemoryRequestMessagingService : IRequestMessagingService
{
    private static readonly List<ApplicationUser> Users =
    [
        new() { Id = "homeowner-1", DisplayName = "Alex Homeowner", Role = "Homeowner" },
        new() { Id = "board-1", DisplayName = "Bailey Board", Role = "Board" },
    ];

    private static readonly List<Request> Requests =
    [
        new() { Id = 1, Title = "Pool gate issue", Description = "Latch is broken", HomeownerUserId = "homeowner-1", CreatedUtc = DateTime.UtcNow.AddDays(-2) },
    ];

    private static int _messageId;

    [HoaFeature("FullManagement", 2)]
    public Task<IReadOnlyList<RequestSummaryViewModel>> GetVisibleRequestsAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var isBoard = IsBoardOrAdmin(role);
        var visible = Requests.Where(r => isBoard || r.HomeownerUserId == userId)
            .Select(r => new RequestSummaryViewModel { Id = r.Id, Title = r.Title, Status = r.Status })
            .ToList();
        return Task.FromResult<IReadOnlyList<RequestSummaryViewModel>>(visible);
    }

    [HoaFeature("FullManagement", 2)]
    public Task<RequestDetailViewModel?> GetRequestDetailsAsync(int requestId, string userId, string role, CancellationToken cancellationToken = default)
    {
        var request = Requests.SingleOrDefault(r => r.Id == requestId);
        if (request is null || (!IsBoardOrAdmin(role) && request.HomeownerUserId != userId)) return Task.FromResult<RequestDetailViewModel?>(null);

        var timeline = request.Messages
            .Where(m => !m.IsInternalNote || IsBoardOrAdmin(role))
            .OrderBy(m => m.CreatedUtc)
            .Select(m => new RequestMessageTimelineItemViewModel
            {
                MessageId = m.Id,
                Message = m.Message,
                CreatedUtc = m.CreatedUtc,
                IsInternalNote = m.IsInternalNote,
                UserDisplayName = Users.SingleOrDefault(u => u.Id == m.UserId)?.DisplayName ?? "Unknown"
            }).ToList();

        return Task.FromResult<RequestDetailViewModel?>(new RequestDetailViewModel { Request = request, Timeline = timeline, CanCreateInternalNote = IsBoardOrAdmin(role) });
    }

    [HoaFeature("FullManagement", 2)]
    public Task AddMessageAsync(CreateRequestMessageInputModel input, string userId, string role, CancellationToken cancellationToken = default)
    {
        var request = Requests.Single(r => r.Id == input.RequestId);
        if (!IsBoardOrAdmin(role) && request.HomeownerUserId != userId) throw new UnauthorizedAccessException();
        if (input.IsInternalNote && !IsBoardOrAdmin(role)) throw new UnauthorizedAccessException();

        request.Messages.Add(new RequestMessage
        {
            Id = Interlocked.Increment(ref _messageId),
            RequestId = input.RequestId,
            UserId = userId,
            Message = input.Message.Trim(),
            IsInternalNote = input.IsInternalNote,
            CreatedUtc = DateTime.UtcNow,
            Request = request,
            User = Users.SingleOrDefault(u => u.Id == userId)
        });

        return Task.CompletedTask;
    }

    private static bool IsBoardOrAdmin(string role) => role is "Board" or "Admin";
}