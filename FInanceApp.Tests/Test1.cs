using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinanceApp;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinanceApp.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_Should_Have_Default_Currency()
        {
            var user = new User();
            Assert.IsTrue(Enum.IsDefined(typeof(Currency), user.SelectedCurrency));
        }

        [TestMethod]
        public void User_Should_Have_FinancialData_Instance()
        {
            var user = new User();
            Assert.IsNotNull(user.Data);
        }
    }

    [TestClass]
    public class FinancialDataTests
    {
        [TestMethod]
        public void AddTransaction_Should_Add_Transaction()
        {
            var data = new FinancialData();
            var transaction = new IncomeTransaction
            {
                Date = DateTime.Now
            };
            data.AddTransaction(transaction);

            Assert.IsTrue(data.GetTransactionsForMonth(DateTime.Now).Contains(transaction));
        }

        [TestMethod]
        public void RemoveTransaction_Should_Remove_Existing_Transaction()
        {
            var data = new FinancialData();
            var transaction = new ExpenseTransaction();
            data.AddTransaction(transaction);
            var id = transaction.Id;

            data.RemoveTransaction(id);
            Assert.IsFalse(data.GetTransactionsForMonth(DateTime.Now).Contains(transaction));
        }

        [TestMethod]
        public void EditTransaction_Should_Change_Transaction()
        {
            var data = new FinancialData();
            var testDate = DateTime.Now;

            var original = new ExpenseTransaction { Amount = 10, Category = "Food", Date = testDate };
            data.AddTransaction(original);

            var updated = new ExpenseTransaction { Amount = 20, Category = "Transport", Date = testDate };
            data.EditTransaction(original.Id, updated);

            var transactions = data.GetTransactionsForMonth(testDate);
            Assert.AreEqual(20, transactions[0].Amount);
            Assert.AreEqual("Transport", transactions[0].Category);
        }

        [TestMethod]
        public void AddBudget_Should_Store_Budget()
        {
            var data = new FinancialData();
            var budget = new Budget { Month = new DateTime(2025, 5, 1) };

            data.AddBudget(budget);
            Assert.AreEqual(budget, data.GetBudgetForMonth(new DateTime(2025, 5, 1)));
        }

        [TestMethod]
        public void RemoveBudget_Should_Remove_Budget()
        {
            var data = new FinancialData();
            var budget = new Budget { Month = new DateTime(2025, 5, 1) };

            data.AddBudget(budget);
            data.RemoveBudget(new DateTime(2025, 5, 1));

            Assert.IsNull(data.GetBudgetForMonth(new DateTime(2025, 5, 1)));
        }

        [TestMethod]
        public void SaveToJson_Should_Create_File()
        {
            var data = new FinancialData();
            string path = "test_save.json";
            data.SaveToJson(path);
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
        }

        [TestMethod]
        public void LoadFromJson_Should_Not_Throw()
        {
            var data = new FinancialData();
            string path = "test_load.json";
            File.WriteAllText(path, "{}");
            data.LoadFromJson(path);
            File.Delete(path);
        }
    }

    [TestClass]
    public class TransactionTests
    {
        [TestMethod]
        public void IncomeTransaction_Should_Return_Income_Type()
        {
            var income = new IncomeTransaction();
            Assert.AreEqual("Income", income.GetTransactionType());
        }

        [TestMethod]
        public void ExpenseTransaction_Should_Return_Expense_Type()
        {
            var expense = new ExpenseTransaction();
            Assert.AreEqual("Expense", expense.GetTransactionType());
        }

        [TestMethod]
        public void Validate_Should_Not_Throw_For_Default_Transaction()
        {
            var income = new IncomeTransaction
            {
                Amount = 0,
                Date = DateTime.Now,
                Category = "Other"
            };

            income.Validate();
        }
    }

    [TestClass]
    public class BudgetTests
    {
        [TestMethod]
        public void AddTransaction_Should_Increase_Expenses()
        {
            var budget = new Budget { Limit = 100 };
            budget.AddTransaction(0);
            budget.AddTransaction(50);
            Assert.AreEqual(50, budget.Expenses);
        }

        [TestMethod]
        public void RemoveTransaction_Should_Decrease_Expenses()
        {
            var budget = new Budget { Limit = 100 };
            budget.AddTransaction(50);
            budget.RemoveTransaction(20);
            Assert.AreEqual(30, budget.Expenses);
        }

        [TestMethod]
        public void IsExceeded_Should_Return_True_When_Over_Limit()
        {
            var budget = new Budget { Limit = 100 };
            budget.AddTransaction(150);
            Assert.IsTrue(budget.IsExceeded());
        }

        [TestMethod]
        public void CheckLimit_Should_Trigger_Event_When_Exceeded()
        {
            var budget = new Budget { Limit = 100 };
            budget.AddTransaction(150);

            bool triggered = false;
            budget.BudgetExceeded += (s, e) => triggered = true;

            budget.CheckLimit();
            Assert.IsTrue(triggered);
        }
    }

    [TestClass]
    public class ReportTests
    {
        [TestMethod]
        public void GetSummary_Should_Return_String()
        {
            var report = new Report();
            var summary = report.GetSummary();
            Assert.IsFalse(string.IsNullOrEmpty(summary));
        }
    }

    [TestClass]
    public class ReportGeneratorTests
    {
        [TestMethod]
        public void GenerateReport_Should_Return_Report()
        {
            var user = new User();
            var report = ReportGenerator.GenerateReport(user, 5, 2025, ReportType.Monthly);
            Assert.IsNotNull(report);
        }
    }
}