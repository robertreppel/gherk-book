using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookkeeper;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;
using NUnit.Framework;
using SharpTestsEx;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TestBookkeeper
{
    [Binding]
    public class TrialBalanceValidationStepDefinition
    {
        [BeforeScenario]
        public void InitializeAccountingSystem()
        {
            var business = Business.SetUpAccounting();
            ScenarioContext.Current.Add("business", business);
        }

        [Given(@"a (asset|liability|revenue|expense|equity) account (\d+) ""(.*)""")]
        public void GivenAnAccountOfType(string accountDescription, int accountNo, string accountName)
        {
            var business = (IDoAccounting) ScenarioContext.Current["business"];
            business.CreateNewAccount(accountNo, accountName, AccountTypeFrom(accountDescription));
        }

        [When(@"I purchase (.*) \(acct\. (\d+)\) for \$(\d+) \+ \$(\d+) tax from ""(.*)"" \(acct\. (\d+)\)")]
        public void WhenIMakeAPurchase(string assetAccountName, int assetAccountNo, decimal netAmount, decimal salesTax, string supplierName, int supplierAccountNo)
        {
            var business = (IDoAccounting)ScenarioContext.Current["business"];
            business.RecordPurchaseFrom(supplierAccountNo, assetAccountNo, netAmount, salesTax, DateTime.Now, supplierName);
        }

        [Then(@"the trial balance should look like this:")]
        public void ThenTheTrialBalanceShouldLookLikeThis(Table table)
        {
            var expectedTrialBalance = TrialBalanceTransform(table);

            var business = (IDoAccounting) ScenarioContext.Current["business"];
            var trialBalance = business.GetTrialBalance();

            var reports = ReportPrinter.For(business);
            reports.Print<ITrialBalance>();

            Compare(expectedTrialBalance, trialBalance.LineItems);
        }

        [Then(@"the trial balance total should be \$(\d+)\.")]
        public void ThenTheTrialBalanceTotalShouldBe(decimal expectedTrialBalanceTotal)
        {
            var business = (IDoAccounting)ScenarioContext.Current["business"];
            var trialBalance = business.GetTrialBalance();
            trialBalance.IsBalanced.Should().Be.True();
            trialBalance.TotalCreditAmount.Should().Be(expectedTrialBalanceTotal);
            trialBalance.TotalDebitAmount.Should().Be(expectedTrialBalanceTotal);
        }

        private static void Compare(IEnumerable<ITrialBalanceLineItem> expectedTrialBalanceLineItems, IEnumerable<ITrialBalanceLineItem> receivedLineItems)
        {
            Assert.AreEqual(expectedTrialBalanceLineItems.Count(), receivedLineItems.Count(),
                            "Expected trial balance lineitem count differs from received trial balance.");

            foreach (var trialBalanceLineItem in expectedTrialBalanceLineItems)
            {
                var expectedLineItem = trialBalanceLineItem;

                var received = (from r in receivedLineItems
                                        where r.AccountNumber == expectedLineItem.AccountNumber
                                        select r).FirstOrDefault();
                Assert.IsNotNull(received, "Expected account " + expectedLineItem.AccountNumber + " not found in received trial balance.");              
                Assert.AreEqual(received.AccountName, expectedLineItem.AccountName);
                Assert.AreEqual(received.AcctType, expectedLineItem.AcctType);
                Assert.AreEqual(received.Credit, expectedLineItem.Credit);
                Assert.AreEqual(received.Debit, expectedLineItem.Debit);
            }
        }

        [StepArgumentTransformation]
        public IEnumerable<ITrialBalanceLineItem> TrialBalanceTransform(Table trialBalance)
        {
            var testTrialBalanceLineItems = trialBalance.CreateSet<TestTrialBalanceLineItem>();
            return testTrialBalanceLineItems;

        }

        internal class TestTrialBalanceLineItem : ITrialBalanceLineItem
        {
            public TestTrialBalanceLineItem()
            {
                
            }

            public int AccountNumber { get; set; }
            public string AccountName { get; set; }
            public AccountType AcctType { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }

        private static AccountType AccountTypeFrom(string accountTypeString)
        {
            switch (accountTypeString.ToLower())
            {
                case "asset":
                    return AccountType.Asset;
                case "liability":
                    return AccountType.Liability;
                case "revenue":
                    return AccountType.Revenue;
                case "expense":
                    return AccountType.Expense;
                case "equity":
                    return AccountType.Equity;
                default:
                    throw new ArgumentException("Unknown account type '" + accountTypeString + "'");
            }
        }
    }
}
