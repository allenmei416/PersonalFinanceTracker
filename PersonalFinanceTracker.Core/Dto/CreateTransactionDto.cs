namespace PersonalFinanceTracker.Core.Dto
{
    public class CreateTransactionDto
    {
        public CreateTransactionDto(int categoryId, decimal amount, DateTime date, string? note)
        {
            CategoryId = categoryId;
            Amount = amount;
            Date = date;
            Note = note;
        }

        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }
    }

}
