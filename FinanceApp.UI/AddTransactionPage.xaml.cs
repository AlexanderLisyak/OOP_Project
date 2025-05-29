using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FinanceApp.UI.ViewModels;

namespace FinanceApp.UI
{
    public partial class AddTransactionPage : Page
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Frame _mainFrame;

        private readonly List<string> _expenseCategories = new()
        {
            "Food", "Eating Out", "Transport", "Purchases", "Utilities", "Entertainment", "Services"
        };

        private readonly List<string> _incomeCategories = new()
        {
            "Salary", "Gift", "Transfer"
        };

        public AddTransactionPage(MainViewModel viewModel, Frame mainFrame)
        {
            InitializeComponent();
            _mainViewModel = viewModel;
            _mainFrame = mainFrame;

            this.Loaded += AddTransactionPage_Loaded;
        }

        private void AddTransactionPage_Loaded(object sender, RoutedEventArgs e)
        {
            FillCategories(isIncome: false);
        }

        private void TransactionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selected = selectedItem.Content.ToString();
                FillCategories(isIncome: selected == "Income");
            }
        }

        private void FillCategories(bool isIncome)
        {
            if (CategoryComboBox == null)
                return;

            CategoryComboBox.ItemsSource = isIncome ? _incomeCategories : _expenseCategories;

            if (CategoryComboBox.Items.Count > 0)
                CategoryComboBox.SelectedIndex = 0;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            
            if (!decimal.TryParse(AmountTextBox.Text, out var amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (CategoryComboBox.SelectedItem is not string category || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            var date = TransactionDatePicker.SelectedDate ?? DateTime.Today;
            if (date > DateTime.Today)
            {
                MessageBox.Show("The date cannot be in the future.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            var isIncome = ((ComboBoxItem)TransactionTypeComboBox.SelectedItem).Content.ToString() == "Income";

            AbstractTransaction transaction = isIncome
                ? new IncomeTransaction(amount, date, category)
                : new ExpenseTransaction(amount, date, category);

            _mainViewModel.AddTransaction(transaction);
            _mainFrame.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }
    }
}