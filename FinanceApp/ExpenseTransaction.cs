using FinanceApp;

public class ExpenseTransaction : AbstractTransaction
{
    public override string GetTransactionType() => "Expense";

    public override bool IsIncome => false;
}
