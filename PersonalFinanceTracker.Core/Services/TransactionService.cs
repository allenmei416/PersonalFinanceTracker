using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Data.Models;

namespace PersonalFinanceTracker.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _db;

        public TransactionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            return await _db.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category!.Name,
                    Amount = t.Amount,
                    Date = t.Date,
                    Note = t.Note
                })
                .ToListAsync();
        }

        // GET BY ID
        public async Task<TransactionDto?> GetByIdAsync(int id)
        {
            var t = await _db.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (t == null) return null;

            return new TransactionDto
            {
                TransactionId = t.TransactionId,
                CategoryId = t.CategoryId,
                CategoryName = t.Category!.Name,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note
            };
        }

        // CREATE
        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            var transaction = new Transaction
            {
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Date = dto.Date,
                Note = dto.Note
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return new TransactionDto
            {
                TransactionId = transaction.TransactionId,
                CategoryId = transaction.CategoryId,
                CategoryName = (await _db.Categories.FindAsync(dto.CategoryId))?.Name ?? "",
                Amount = transaction.Amount,
                Date = transaction.Date,
                Note = transaction.Note
            };
        }

        // UPDATE
        public async Task<TransactionDto> UpdateAsync(int id, UpdateTransactionDto dto)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null)
                throw new KeyNotFoundException("Transaction not found");

            transaction.CategoryId = dto.CategoryId;
            transaction.Amount = dto.Amount;
            transaction.Date = dto.Date;
            transaction.Note = dto.Note;

            await _db.SaveChangesAsync();

            return new TransactionDto
            {
                TransactionId = transaction.TransactionId,
                CategoryId = transaction.CategoryId,
                CategoryName = (await _db.Categories.FindAsync(dto.CategoryId))?.Name ?? "",
                Amount = transaction.Amount,
                Date = transaction.Date,
                Note = transaction.Note
            };
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null) throw new KeyNotFoundException("Transaction not found");

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
