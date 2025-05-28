using FinanceApp;

public class ExpenseTransaction : AbstractTransaction
{
    public ExpenseTransaction(decimal amount, DateTime date, string category)
    {
        Amount = amount;
        Date = date;
        Category = category;
    }
    public override string GetTransactionType() => "Expense";

    public override bool IsIncome => false;
}