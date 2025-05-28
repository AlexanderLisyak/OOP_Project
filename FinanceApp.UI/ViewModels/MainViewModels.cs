using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

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
                .ToDictionary(g => g.Key, g => Math.Abs(g.Sum(t => t.Amount)));
        }

        public void AddTransaction(AbstractTransaction transaction)
        {
            transaction.Validate();

            Transactions.Add(transaction);
            RecentTransactions.Insert(0, transaction);

            if (transaction is IncomeTransaction)
                TotalBalance += transaction.Amount;
            else
                TotalBalance -= transaction.Amount;

            if (!_categoryTotals.ContainsKey(transaction.Category))
                _categoryTotals[transaction.Category] = 0;

            _categoryTotals[transaction.Category] += transaction.IsIncome
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
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var data = new JsonData
            {
                Currency = SelectedCurrency,
                Transactions = Transactions.ToList()
            };

            var json = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            var json = File.ReadAllText(path);
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new TransactionJsonConverter() }
            };

            var data = JsonConvert.DeserializeObject<JsonData>(json, settings);

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

            [JsonProperty(ItemConverterType = typeof(TransactionJsonConverter))]
            public List<AbstractTransaction> Transactions { get; set; } = new();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}