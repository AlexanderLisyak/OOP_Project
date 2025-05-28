using FinanceApp;

namespace FinanceApp
{
    public static class ReportGenerator
    {
        public static Report GenerateReport(User user, int month, int year, ReportType type)
        {
            var transactions = user.Data.GetTransactionsForMonth(new DateTime(year, month, 1));

            var report = new Report
            {
                Period = new DateTime(year, month, 1),
                TotalIncome = transactions.Where(t => t.GetTransactionType() == "Income").Sum(t => t.Amount),
                TotalExpenses = transactions.Where(t => t.GetTransactionType() == "Expense").Sum(t => t.Amount)
            };

            foreach (var expense in transactions.Where(t => t.GetTransactionType() == "Expense"))
            {
                if (!report.ExpensesByCategory.ContainsKey(expense.Category))
                    report.ExpensesByCategory[expense.Category] = 0;
                report.ExpensesByCategory[expense.Category] += expense.Amount;
            }

            return report;
        }
    }
}