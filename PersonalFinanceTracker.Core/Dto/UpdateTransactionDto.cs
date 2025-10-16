namespace PersonalFinanceTracker.Core.Dto
{
    public class UpdateTransactionDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
    }

}
