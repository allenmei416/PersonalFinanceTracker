using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();

        Task<TransactionDto?> GetByIdAsync(int id);

        Task<TransactionDto> CreateAsync(CreateTransactionDto dto);

        Task<TransactionDto> UpdateAsync(int id, UpdateTransactionDto dto);

        Task DeleteAsync(int id);

    }
}
