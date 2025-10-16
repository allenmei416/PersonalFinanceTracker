namespace PersonalFinanceTracker.Core.Dto
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Expense"; // "Income" or "Expense"
    }

}
