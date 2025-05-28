using FinanceApp;

public class IncomeTransaction : AbstractTransaction
{
    public IncomeTransaction(decimal amount, DateTime date, string category)
    {
        Amount = amount;
        Date = date;
        Category = category;
    }
    public override string GetTransactionType() => "Income";

    public override bool IsIncome => true;
}