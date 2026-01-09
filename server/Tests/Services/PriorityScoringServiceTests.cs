using InboxEngine.Models;
using InboxEngine.Services;
using Xunit;

namespace InboxEngine.Tests.Services;

public class PriorityScoringServiceTests
{
    private readonly IPriorityScoringService _service =
        new PriorityScoringService();

    [Fact]
    public void CalculatePriorityScore_VipEmail_Adds50Points()
    {
        var email = new Email
        {
            IsVIP = true,
            Subject = "Hello",
            Body = "Normal",
            ReceivedAt = DateTime.UtcNow
        };

        var score = _service.CalculatePriorityScore(email);

        Assert.Equal(50, score);
    }

    [Theory]
    [InlineData("Urgent: please read")]
    [InlineData("Need this ASAP")]
    [InlineData("System ERROR occurred")]
    [InlineData("uRgEnT mixed case")]
    public void CalculatePriorityScore_UrgencyKeyword_Adds30Points(string subject)
    {
        var email = new Email
        {
            Subject = subject,
            Body = "Normal",
            ReceivedAt = DateTime.UtcNow
        };

        var score = _service.CalculatePriorityScore(email);

        Assert.Equal(30, score);
    }

    [Fact]
    public void CalculatePriorityScore_TimeDecay_AddsOnePointPerHour()
    {
        var email = new Email
        {
            Subject = "Hello",
            Body = "Normal",
            ReceivedAt = DateTime.UtcNow.AddHours(-6.9) // floor => 6
        };

        var score = _service.CalculatePriorityScore(email);

        Assert.Equal(6, score);
    }

    [Theory]
    [InlineData("Unsubscribe here")]
    [InlineData("Weekly Newsletter")]
    [InlineData("newsletter unsubscribe")]
    public void CalculatePriorityScore_SpamKeywords_Subtracts20AndClampsToZero(string body)
    {
        var email = new Email
        {
            Subject = "Hello",
            Body = body,
            ReceivedAt = DateTime.UtcNow
        };

        var score = _service.CalculatePriorityScore(email);

        Assert.Equal(0, score);
    }

    [Fact]
    public void CalculatePriorityScore_ClampsTo100_WhenScoreExceedsMax()
    {
        var email = new Email
        {
            IsVIP = true,                       // +50
            Subject = "URGENT ASAP ERROR",       // +30
            Body = "Normal",
            ReceivedAt = DateTime.UtcNow.AddHours(-60) // +60
        };

        var score = _service.CalculatePriorityScore(email);

        Assert.Equal(100, score);
    }
}
