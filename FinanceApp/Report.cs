using System;
using System.Collections.Generic;

namespace FinanceApp
{
    public class Report
    {
        public DateTime Period { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new();

        public string GetSummary()
        {
            return $"Period: {Period.ToShortDateString()}, Income: {TotalIncome}, Expenses: {TotalExpenses}";
        }
    }
}