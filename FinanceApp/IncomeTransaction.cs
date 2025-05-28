using FinanceApp;

public class IncomeTransaction : AbstractTransaction
{
    public override string GetTransactionType() => "Income";

    public override bool IsIncome => true;
}
