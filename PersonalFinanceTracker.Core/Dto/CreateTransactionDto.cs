namespace PersonalFinanceTracker.Core.Dto
{
    public class CreateTransactionDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }
    }

}
