using System;

namespace FinanceApp
{
    public class Budget
    {
        public DateTime Month { get; set; }
        public decimal Limit { get; set; }
        public decimal Expenses { get; private set; }

        public event EventHandler? BudgetExceeded;

        public void AddTransaction(decimal amount)
        {
            Expenses += amount;
            CheckLimit();
        }

        public void RemoveTransaction(decimal amount)
        {
            Expenses -= amount;
            if (Expenses < 0) Expenses = 0;
        }

        public void CheckLimit()
        {
            if (Expenses > Limit)
            {
                BudgetExceeded?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsExceeded()
        {
            return Expenses > Limit;
        }
    }
}