using InboxEngine.Models;

namespace InboxEngine.Services;
public sealed class InboxService : IInboxService
{
    private readonly IPriorityScoringService _scoringService;

    public InboxService(IPriorityScoringService scoringService)
    {
        _scoringService = scoringService;
    }

    public IReadOnlyList<Email> SortByPriority(IEnumerable<Email> emails)
    {
        foreach (var email in emails)
            email.PriorityScore = _scoringService.CalculatePriorityScore(email);

        return emails
            .OrderByDescending(e => e.PriorityScore)
            .ThenByDescending(e => e.ReceivedAt)
            .ToList();
    }
}
