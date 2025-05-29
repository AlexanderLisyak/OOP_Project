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
            var transaction = new IncomeTransaction(100, DateTime.Now, "Salary");
            data.AddTransaction(transaction);

            Assert.IsTrue(data.GetTransactionsForMonth(DateTime.Now).Contains(transaction));
        }

        [TestMethod]
        public void RemoveTransaction_Should_Remove_Existing_Transaction()
        {
            var data = new FinancialData();
            var transaction = new ExpenseTransaction(50, DateTime.Now, "Food");
            data.AddTransaction(transaction);
            var id = transaction.Id;

            data.RemoveTransaction(id);
            Assert.IsFalse(data.GetTransactionsForMonth(DateTime.Now).Contains(transaction));
        }

        [TestMethod]
        public void SaveToJson_Should_Create_File()
        {
            var data = new FinancialData();
            var transaction = new IncomeTransaction(120, DateTime.Now, "Test");
            data.AddTransaction(transaction);

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
            File.WriteAllText(path, "{\"Transactions\":[]}");
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
            var income = new IncomeTransaction(150, DateTime.Now, "Gift");
            Assert.AreEqual("Income", income.GetTransactionType());
        }

        [TestMethod]
        public void ExpenseTransaction_Should_Return_Expense_Type()
        {
            var expense = new ExpenseTransaction(70, DateTime.Now, "Transport");
            Assert.AreEqual("Expense", expense.GetTransactionType());
        }

        [TestMethod]
        public void Validate_Should_Throw_For_Invalid_Amount()
        {
            var income = new IncomeTransaction(0, DateTime.Now, "Gift");
            Assert.ThrowsException<InvalidOperationException>(() => income.Validate());
        }

        [TestMethod]
        public void Validate_Should_Throw_For_Empty_Category()
        {
            var expense = new ExpenseTransaction(50, DateTime.Now, "");
            Assert.ThrowsException<InvalidOperationException>(() => expense.Validate());
        }

        [TestMethod]
        public void Validate_Should_Not_Throw_For_Valid_Transaction()
        {
            var expense = new ExpenseTransaction(100, DateTime.Now, "Utilities");
            expense.Validate();
        }
    }
}