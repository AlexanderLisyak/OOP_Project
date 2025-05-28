using System;
using Newtonsoft.Json;

namespace FinanceApp
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractTransaction
    {
        [JsonProperty] public Guid Id { get; set; } = Guid.NewGuid();
        [JsonProperty] public decimal Amount { get; set; }
        [JsonProperty] public DateTime Date { get; set; }
        [JsonProperty] public string Category { get; set; }

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