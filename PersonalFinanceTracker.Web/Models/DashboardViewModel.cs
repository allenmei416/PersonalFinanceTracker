using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Web.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }
        public CreateTransactionDto NewTransaction { get; set; }
    }
}
