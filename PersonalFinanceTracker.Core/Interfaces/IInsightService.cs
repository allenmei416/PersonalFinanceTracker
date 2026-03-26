using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IInsightService
    {
        Task<string> GetInsightAsync(IEnumerable<TransactionDto> transactions);
    }
}
