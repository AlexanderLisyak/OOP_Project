namespace FinanceApp
{
    public class User
    {
        public Currency SelectedCurrency { get; set; }
        public FinancialData Data { get; set; } = new FinancialData();

        public void ChangeCurrency(Currency newCurrency)
        {
            SelectedCurrency = newCurrency;
        }
    }
}