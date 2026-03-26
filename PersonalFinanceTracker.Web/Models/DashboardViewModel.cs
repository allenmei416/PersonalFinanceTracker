using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Web.Models
{
    public class DashboardViewModel
    {
        public List<TransactionDto> Transactions { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public Dictionary<string, decimal> SpendingByCategory { get; set; } = new();
        public Dictionary<string, decimal> MonthlySpend { get; set; } = new();
        public string AiInsight { get; set; } = string.Empty;
    }
}
