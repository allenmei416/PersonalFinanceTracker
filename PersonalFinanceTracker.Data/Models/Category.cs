
namespace PersonalFinanceTracker.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Transaction>? Transactions { get; set; }
    }

}
