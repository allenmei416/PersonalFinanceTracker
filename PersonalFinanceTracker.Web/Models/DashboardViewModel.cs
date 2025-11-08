using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Web.Models
{
    public class DashboardViewModel
    {
        public List<TransactionDto> Transactions { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
    }
}
