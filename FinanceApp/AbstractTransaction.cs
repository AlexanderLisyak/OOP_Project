using System;

namespace FinanceApp
{
    public abstract class AbstractTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }

        public abstract string GetTransactionType();

        public abstract bool IsIncome { get; }

        public virtual void Validate()
        {
            if (Amount < 0)
                throw new InvalidOperationException("Amount cannot be negative");
            if (string.IsNullOrWhiteSpace(Category))
                throw new InvalidOperationException("Category is required");
        }
    }
}