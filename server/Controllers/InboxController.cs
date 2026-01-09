using Microsoft.AspNetCore.Mvc;
using InboxEngine.Models;
using InboxEngine.Services;

namespace InboxEngine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InboxController : ControllerBase
{
    private readonly IPriorityScoringService _scoringService;
    private readonly ILogger<InboxController> _logger;

    public InboxController(
        IPriorityScoringService scoringService,
        ILogger<InboxController> logger)
    {
        _scoringService = scoringService;
        _logger = logger;
    }

    [HttpPost("sort")]
    public IActionResult SortEmails([FromBody] List<Email> emails)
    {
        // Validate input
        if (emails == null || emails.Count == 0)
        {
            _logger.LogInformation("SortEmails called with empty or null email list.");
            return Ok(Array.Empty<Email>());
        }

        // Calculate priority score for each email
        foreach (var email in emails)
        {
            email.PriorityScore = _scoringService.CalculatePriorityScore(email);
        }

        // Sort by PriorityScore
        var sortedEmails = emails
            .OrderByDescending(e => e.PriorityScore)
            .ThenByDescending(e => e.ReceivedAt)
            .ToList();

        return Ok(sortedEmails);
    }
}
