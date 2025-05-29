using System;
using System.Collections.Generic;
using System.Text.Json;
using FinanceApp;

namespace FinanceApp
{
    public class FinancialData : IJsonSerializable
    {
        private List<AbstractTransaction> Transactions { get; set; } = new();

        public void SaveToJson(string path)
        {
            var json = JsonSerializer.Serialize(new { Transactions});
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


        public List<AbstractTransaction> GetTransactionsForMonth(DateTime month)
        {
            return Transactions.Where(t => t.Date.Year == month.Year && t.Date.Month == month.Month).ToList();
        }

        public List<AbstractTransaction> GetAllTransactions()
        {
            return new List<AbstractTransaction>(Transactions);
        }

        private class FinancialDataJsonModel
        {
            public List<AbstractTransaction>? Transactions { get; set; }
        }
    }
}