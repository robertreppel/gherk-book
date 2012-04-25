using System.Collections.Generic;
using System.Linq;
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
        [Then(@"the trial balance should look like this:")]
        public void ThenTheTrialBalanceShouldLookLikeThis(Table table)
        {
            var expectedTrialBalanceLineItems = TrialBalanceTransform(table);

            var business = (IDoAccounting)ScenarioContext.Current["business"];

            //For debugging - uncomment to see what the actual trial balance looks like:
            //var reports = ReportPrinter.For(business);
            //reports.Print<ITrialBalance>();

            var actualTrialBalance = business.GetTrialBalance();
            Compare(expectedTrialBalanceLineItems, actualTrialBalance.LineItems);
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

        private static void Compare(IEnumerable<ITrialBalanceLineItem> expectedTrialBalanceLineItems, IEnumerable<ITrialBalanceLineItem> actualLineItems)
        {
            Assert.AreEqual(expectedTrialBalanceLineItems.Count(), actualLineItems.Count(),
                            "Expected trial balance lineitem count differs from received trial balance.");

            foreach (var trialBalanceLineItem in expectedTrialBalanceLineItems)
            {
                var expectedLineItem = trialBalanceLineItem;

                var actual = (from item in actualLineItems
                                        where item.AccountNumber == expectedLineItem.AccountNumber
                                        select item).FirstOrDefault();
                Assert.IsNotNull(actual, "Expected account " + expectedLineItem.AccountNumber + " not found in received trial balance.");              
                Assert.AreEqual(actual.AccountName, expectedLineItem.AccountName);
                Assert.AreEqual(actual.AcctType, expectedLineItem.AcctType);
                Assert.AreEqual(actual.Credit, expectedLineItem.Credit);
                Assert.AreEqual(actual.Debit, expectedLineItem.Debit);
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
            public int AccountNumber { get; set; }
            public string AccountName { get; set; }
            public AccountType AcctType { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }
    }
}
