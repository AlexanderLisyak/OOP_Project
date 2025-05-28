using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FinanceApp
{
    public class FinancialData : IJsonSerializable
    {
        private List<AbstractTransaction> Transactions { get; set; } = new();
        private List<Budget> Budgets { get; set; } = new();

        public void SaveToJson(string path)
        {
            var json = JsonSerializer.Serialize(new { Transactions, Budgets });
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<FinancialDataJsonModel>(json);
                if (data != null)
                {
                    Transactions = data.Transactions ?? new List<AbstractTransaction>();
                    Budgets = data.Budgets ?? new List<Budget>();
                }
            }
        }

        public void AddTransaction(AbstractTransaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void RemoveTransaction(Guid transactionId)
        {
            Transactions.RemoveAll(t => t.Id == transactionId);
        }

        public void EditTransaction(Guid transactionId, AbstractTransaction updatedTransaction)
        {
            int index = Transactions.FindIndex(t => t.Id == transactionId);
            if (index >= 0)
                Transactions[index] = updatedTransaction;
            else
                throw new ArgumentException("Transaction not found");
        }

        public void AddBudget(Budget budget)
        {
            Budgets.Add(budget);
        }

        public void RemoveBudget(DateTime month)
        {
            Budgets.RemoveAll(b => b.Month.Year == month.Year && b.Month.Month == month.Month);
        }

        public Budget? GetBudgetForMonth(DateTime month)
        {
            return Budgets.FirstOrDefault(b => b.Month.Year == month.Year && b.Month.Month == month.Month);
        }

        public List<AbstractTransaction> GetTransactionsForMonth(DateTime month)
        {
            return Transactions.Where(t => t.Date.Year == month.Year && t.Date.Month == month.Month).ToList();
        }

        private class FinancialDataJsonModel
        {
            public List<AbstractTransaction>? Transactions { get; set; }
            public List<Budget>? Budgets { get; set; }
        }
    }
}