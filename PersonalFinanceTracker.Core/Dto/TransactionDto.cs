namespace PersonalFinanceTracker.Core.Dto
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
    }

}
