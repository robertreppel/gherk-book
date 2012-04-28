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
            var business = Business.SetUpAccounting();
            business.Bookkeeper.CreateNewAccount(1001, "Bank Account", AccountType.Asset);


            var chartOfAccounts = business.Bookkeeper.GetChartOfAccounts().ToList();

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();

            //Created account + other accounts generated in CreateAndSetUpAccounts:
            chartOfAccounts.Count.Should().Be(4);            
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Account no. 1001 already exists in the ledger.")]
        public void ShouldNotCreateASecondAccountWithTheSameNumber()
        {
            var business = Business.SetUpAccounting();
            business.Bookkeeper.CreateNewAccount(1001, "Bank Account", AccountType.Asset);
            business.Bookkeeper.CreateNewAccount(1001, "Another Account", AccountType.Asset);
        }

        [Test]
        public void ShouldRecordCashSaleWithoutSalesTax()
        {
            var business = Business.SetUpAccounting();
            const int customer = 2000;
            business.Bookkeeper.CreateNewAccount(customer, "Jim Haskell", AccountType.Revenue);

            const decimal totalAmount = 124.0m;

            business.RecordTaxFreeSale(customer, totalAmount,DateTime.Today, "item sold");

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();

            var journal = business.Bookkeeper.GetJournal().ToList();
            journal.Count.Should().Be(2);
        }

        [Test]
        public void ShouldChargeSalesTax()
        {
            var business = Business.SetUpAccounting();
            const int customer = 2001;
            business.Bookkeeper.CreateNewAccount(customer, "Jemima Jenkins", AccountType.Revenue);
            
            const decimal netAmount = 1200.13m;
            const decimal salesTaxAmount = 120.1m;
            business.RecordTaxableSale(customer, netAmount, salesTaxAmount, DateTime.Today, "item");

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();

            var trialBalance = business.Bookkeeper.GetTrialBalance();
            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");
            trialBalance.TotalCreditAmount.Should().Be(netAmount + salesTaxAmount);
            trialBalance.TotalDebitAmount.Should().Be(netAmount + salesTaxAmount);
        }

        [Test]
        public void ShouldRecordPurchaseWithSalesTax()
        {
            var business = Business.SetUpAccounting();
            const int supplier = 600;
            business.Bookkeeper.CreateNewAccount(supplier, "Marvin's Office Supplies", AccountType.Liability);

            const int officeSupplies = 9000;
            business.Bookkeeper.CreateNewAccount(officeSupplies, "Office Supplies", AccountType.Asset);

            const decimal netAmount = 80.0m;
            const decimal salesTaxAmount = 8.0m;
            business.RecordPurchaseFrom(supplier, officeSupplies, netAmount, salesTaxAmount, DateTime.Today, "item bought");

            var trialBalance = business.Bookkeeper.GetTrialBalance();
            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();

            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");
        }

        [Test]
        public void ShouldPayAccountsPayable()
        {
            var business = Business.SetUpAccounting();
            const int officeSupplies = 9000;

            business.Bookkeeper.CreateNewAccount(officeSupplies, "Office Supplies", AccountType.Asset);

            const int acmeEnterprises = 2005;
            business.Bookkeeper.CreateNewAccount(acmeEnterprises, "Acme Enterprises, Inc.", AccountType.Revenue);
            const int thelmasTonerShack = 602;
            business.Bookkeeper.CreateNewAccount(thelmasTonerShack, "Thelma's Toner Shack", AccountType.Liability);

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

            business.RecordPaymentTo(thelmasTonerShack, PartialPaymentOf(40.0m), DateTime.Today, "Payment for Inv. 1234");

            var trialBalance = business.Bookkeeper.GetTrialBalance();

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();
            reports.Print<IAccount>(thelmasTonerShack);
            reports.Print<IAccount>(officeSupplies);
            reports.Print<IAccount>(business.CashRegisterAcctNo);
            reports.Print<IAccount>(business.SalesTaxOwingAcctNo);

            Assert.IsTrue(trialBalance.IsBalanced, "Accounts should balance.");

        }

        private static decimal PartialPaymentOf(decimal amount)
        {
            return amount;
        }

        [Test]
        public void ShouldCalculateRevenueAndAssetAccountBalances()
        {
            var business = Business.SetUpAccounting();
            const int customer = 6654;
            business.Bookkeeper.CreateNewAccount(customer, "Higgins Farm Machinery, Inc.", AccountType.Revenue);

            business.RecordTaxFreeSale(6654, 1200.0m, DateTime.Now, "IT Consulting Services");

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();

            var cash = business.Bookkeeper.GetAccount(business.CashRegisterAcctNo);
            cash.Balance.Should().Be(1200.0m);

            var customerAccount = business.Bookkeeper.GetAccount(customer);
            customerAccount.Balance.Should().Be(1200);
        }

        [Test]
        public void ShouldCalculateAssetAndLiabilityAccountBalances()
        {
            var business = Business.SetUpAccounting();
            const int investorMikeLewis = 3450;
            business.Bookkeeper.CreateNewAccount(investorMikeLewis, "Mike Lewis Investment", AccountType.Liability);
            business.RecordCashInvestmentBy(investorMikeLewis, 10000.0m, DateTime.Now, "Grubstake provided by Uncle Mike.");

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();
            reports.Print<IAccount>(investorMikeLewis);

            var cash = business.Bookkeeper.GetAccount(business.CashRegisterAcctNo);
            cash.Balance.Should().Be(10000.0m);

            var mikeLewisAccount = business.Bookkeeper.GetAccount(investorMikeLewis);
            mikeLewisAccount.Balance.Should().Be(10000.0m);
        }

        [Test]
        public void ShouldRecordOwnersEquity()
        {
            var business = Business.SetUpAccounting();
            business.RecordCashInjectionByOwner(5000.0m, DateTime.Now, "John Smith, cash injection into business");

            var ownersEquity = business.Bookkeeper.GetAccount(business.OwnersEquityAcctNo);
            ownersEquity.Balance.Should().Be(5000.0m);

            var cash = business.Bookkeeper.GetAccount(business.CashRegisterAcctNo);
            cash.Balance.Should().Be(5000.0m);

            var reports = ReportPrinter.For(business.Bookkeeper);
            reports.Print<ITrialBalance>();
            reports.Print<IAccount>(business.OwnersEquityAcctNo);
            reports.Print<IAccount>(business.CashRegisterAcctNo);
        }
    }
}
