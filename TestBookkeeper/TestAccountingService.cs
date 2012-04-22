using System;
using System.Linq;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;
using NUnit.Framework;
using SharpTestsEx;

namespace TestBookkeeper
{
    [TestFixture]
    public class TestAccountingService
    {
        [Test]
        public void ShouldCreateNewAccount()
        {
            var business = AccountingService.SetUpAccounting();
            business.CreateNewAccount(1001, "Bank Account", AccountType.Asset);


            var chartOfAccounts = business.GetChartOfAccounts().ToList();

            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();

            //Created account + other accounts generated in CreateAndSetUpAccounts:
            chartOfAccounts.Count.Should().Be(4);            
        }

        [Test]
        public void ShouldRecordCashSaleWithoutSalesTax()
        {
            var business = AccountingService.SetUpAccounting();
            const int customer = 2000;
            business.CreateNewAccount(customer, "Jim Haskell", AccountType.Revenue);

            const decimal totalAmount = 124.0m;

            business.RecordTaxFreeSale(customer, totalAmount,DateTime.Today, "item sold");

            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();

            var journal = business.GetJournal().ToList();
            journal.Count.Should().Be(2);
        }

        [Test]
        public void ShouldChargeSalesTax()
        {
            var business = AccountingService.SetUpAccounting();
            const int customer = 2001;
            business.CreateNewAccount(customer, "Jemima Jenkins", AccountType.Revenue);

            const decimal netAmount = 1200.13m;
            const decimal salesTaxAmount = 120.1m;
            business.RecordTaxableSale(customer, netAmount, salesTaxAmount, DateTime.Today, "item");

            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();

            var trialBalance = business.GetTrialBalance();
            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");
            trialBalance.TotalCreditAmount.Should().Be(netAmount + salesTaxAmount);
            trialBalance.TotalDebitAmount.Should().Be(netAmount + salesTaxAmount);
        }

        [Test]
        public void ShouldRecordPurchaseWithSalesTax()
        {
            var business = AccountingService.SetUpAccounting();
            const int supplier = 600;
            business.CreateNewAccount(supplier, "Marvin's Office Supplies", AccountType.Liability);

            const int officeSupplies = 9000;
            business.CreateNewAccount(officeSupplies, "Office Supplies", AccountType.Asset);

            const decimal netAmount = 80.0m;
            const decimal salesTaxAmount = 8.0m;
            business.RecordPurchaseFrom(supplier, officeSupplies, netAmount, salesTaxAmount, DateTime.Today, "item bought");

            var trialBalance = business.GetTrialBalance();
            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();

            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");
        
        }

        [Test]
        public void ShouldPayAccountsPayable()
        {
            var business = AccountingService.SetUpAccounting();
            const int cashRegister = 1000;
            const int officeSupplies = 9000;
            const int salesTaxOwing = 3000;
            const int salesTaxPaid = 3001;

            business.CreateNewAccount(officeSupplies, "Office Supplies", AccountType.Asset);

            const int acmeEnterprises = 2005;
            business.CreateNewAccount(acmeEnterprises, "Acme Enterprises, Inc.", AccountType.Revenue);
            const int thelmasTonerShack = 602;
            business.CreateNewAccount(thelmasTonerShack, "Thelma's Toner Shack", AccountType.Liability);

            business.RecordTaxableSale(customerAccountNo: acmeEnterprises, 
                                       netAmount: 100.0m, 
                                       salesTaxAmount: 10.0m, 
                                       transactionDate: DateTime.Today, 
                                       transactionReference: "Plumbing Services sold to Larry (1.5hrs.)");

            business.RecordPurchaseFrom(
                        supplierAccountNo: thelmasTonerShack, 
                        assetAccountNo: officeSupplies, 
                        netAmount: 45.0m, 
                        salesTaxAmount: 5.0m, 
                        transactionDate: DateTime.Today, 
                        transactionReference: "Inv. 1234 - Toner Cartridges (black & blue)");

            business.PayTo(thelmasTonerShack, PartialPaymentOf45Dollars(), DateTime.Today, "Payment for Inv. 1234");

            var trialBalance = business.GetTrialBalance();

            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();
            reports.Print<IAccount>(thelmasTonerShack);
            reports.Print<IAccount>(officeSupplies);
            reports.Print<IAccount>(cashRegister);
            reports.Print<IAccount>(salesTaxOwing);
            reports.Print<IAccount>(salesTaxPaid);

            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");

        }

        private static decimal PartialPaymentOf45Dollars()
        {
            return 40.0m;
        }


    }
}
