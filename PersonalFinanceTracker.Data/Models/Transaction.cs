using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Data.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public Category? Category { get; set; }
    }
}
