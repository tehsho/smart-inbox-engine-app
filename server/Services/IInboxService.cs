using InboxEngine.Models;

namespace InboxEngine.Services;

public interface IInboxService
{
    IReadOnlyList<Email> SortByPriority(IEnumerable<Email> emails);
}
