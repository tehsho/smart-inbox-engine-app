using InboxEngine.Models;

namespace InboxEngine.Services;

/// <summary>
/// TODO: Implement the priority scoring logic according to the requirements:
/// - VIP Status: +50 points if IsVIP is true
/// - Urgency Keywords: +30 points if Subject contains "Urgent", "ASAP", or "Error" (case-insensitive)
/// - Time Decay: +1 point for every hour passed since ReceivedAt
/// - Spam Filter: -20 points if Body contains "Unsubscribe" or "Newsletter"
/// - Clamping: Final score must be between 0 and 100 (inclusive)
/// </summary>
public sealed class PriorityScoringService : IPriorityScoringService
{
    private static readonly string[] UrgencyKeywords =
        ["urgent", "asap", "error"];

    private static readonly string[] SpamKeywords =
        ["unsubscribe", "newsletter"];

    public int CalculatePriorityScore(Email email)
    {
        int score = 0;

        // VIP Status
        if (email.IsVIP)
            score += 50;

        // Urgency keywords in Subject (case-insensitive)
        var subject = (email.Subject ?? string.Empty).ToLowerInvariant();
        if (UrgencyKeywords.Any(k => subject.Contains(k)))
            score += 30;

        // Time decay: +1 point per hour since ReceivedAt
        var receivedUtc = email.ReceivedAt.Kind == DateTimeKind.Utc
            ? email.ReceivedAt
            : email.ReceivedAt.ToUniversalTime();

        var hoursPassed = (int)Math.Floor(
            (DateTime.UtcNow - receivedUtc).TotalHours
        );

        if (hoursPassed > 0)
            score += hoursPassed;

        // Spam filter in Body
        var body = (email.Body ?? string.Empty).ToLowerInvariant();
        if (SpamKeywords.Any(k => body.Contains(k)))
            score -= 20;

        // Clamp 0..100
        if (score < 0) return 0;
        if (score > 100) return 100;

        return score;
    }
}