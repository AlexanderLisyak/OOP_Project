using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Transactions;
using System.IO;
using System.Text.Json;

namespace FinanceApp.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<AbstractTransaction> RecentTransactions { get; set; } = new();
        public ObservableCollection<AbstractTransaction> Transactions { get; set; } = new();

        public List<Currency> AvailableCurrencies { get; } = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();


        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set
            {
                _totalBalance = value;
                OnPropertyChanged(nameof(TotalBalance));
            }
        }

        private Dictionary<string, decimal> _categoryTotals = new();
        public Dictionary<string, decimal> CategoryTotals
        {
            get => _categoryTotals;
            set
            {
                _categoryTotals = value;
                OnPropertyChanged(nameof(CategoryTotals));
            }
        }
        private void CalculateCategoryTotals()
        {
            CategoryTotals = Transactions
                .Where(t => !t.IsIncome) 
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => Math.Abs(t.Amount)));
        }

        public void AddTransaction(AbstractTransaction transaction)
        {
            transaction.Validate();

            Transactions.Add(transaction);
            RecentTransactions.Insert(0, transaction);

            if (transaction is IncomeTransaction)
                TotalBalance += transaction.Amount;
            else if (transaction is ExpenseTransaction)
                TotalBalance -= transaction.Amount;

            if (!_categoryTotals.ContainsKey(transaction.Category))
                _categoryTotals[transaction.Category] = 0;

            _categoryTotals[transaction.Category] += transaction is IncomeTransaction
                ? transaction.Amount
                : -transaction.Amount;

            CalculateCategoryTotals();
        }

        private Currency _selectedCurrency = Currency.USD;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
            }
        }

        public void SaveToJson(string path)
        {
            var data = new
            {
                Currency = SelectedCurrency,
                Transactions = Transactions
            };
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<JsonData>(json);

            if (data != null)
            {
                SelectedCurrency = data.Currency;
                Transactions.Clear();
                RecentTransactions.Clear();

                foreach (var tx in data.Transactions)
                {
                    Transactions.Add(tx);
                    RecentTransactions.Add(tx);
                }

                RecalculateBalance();
            }
        }

        private void RecalculateBalance()
        {
            TotalBalance = Transactions.Sum(t => t.IsIncome ? t.Amount : -t.Amount);
            CalculateCategoryTotals();
        }

        private class JsonData
        {
            public Currency Currency { get; set; }
            public List<AbstractTransaction> Transactions { get; set; } = new();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}