using System;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;
using SharpTestsEx;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TestBookkeeper
{
    [Binding]
    public class AccountValidationStepDefinition
    {
        [Then(@"the ""(.*)"" (\d+) account contains the following:")]
        public void AccountStatementLooksLike(string accountName, int accountNumber, Table table)
        {
            var expectedAccountTransactions = AccountTransform(table);

            var business = (IBusiness) ScenarioContext.Current["business"];

            var account = business.Find<IAccount>(accountNumber);

            Matches(expectedAccountTransactions, account.Transactions).Should().Be(true);
        }

        private static bool Matches(IEnumerable<ITransaction> expectedAccountTransactions, IEnumerable<ITransaction> actualTransactions) {
            if(IndividualTransactionsMatch(expectedAccountTransactions, actualTransactions) && NumberOfTransactionsMatches(expectedAccountTransactions, actualTransactions)) {
                return true;
            }
            return false;
        }

        private static bool NumberOfTransactionsMatches(IEnumerable<ITransaction> expectedAccountTransactions, IEnumerable<ITransaction> actualTransactions) {
            return NoOfLineItemsNotIncludingTheLineShowingTheBalanceIn(expectedAccountTransactions) == actualTransactions.Count();
        }

        private static int NoOfLineItemsNotIncludingTheLineShowingTheBalanceIn(IEnumerable<ITransaction> expectedAccountTransactions) {
            return expectedAccountTransactions.Count() - 1;
        }

        private static bool IndividualTransactionsMatch(IEnumerable<ITransaction> expectedAccountTransactions, IEnumerable<ITransaction> actualTransactions) {
            var matches = true;
            for (var cnt = 0; cnt < expectedAccountTransactions.Count() - 1; cnt++)
            {
                var expected = expectedAccountTransactions.ElementAt(cnt);
                var actualTransaction = (from a in actualTransactions
                                         where a.Credit == expected.Credit
                                               && a.Debit == expected.Debit
                                               && a.TransactionDate == expected.TransactionDate
                                               && a.TransactionReference == expected.TransactionReference
                                         select a).FirstOrDefault();
                if (actualTransaction == null)
                {
                    matches = false;
                }
            }
            return matches;
        }

        [StepArgumentTransformation]
        public IEnumerable<ITransaction> AccountTransform(Table account)
        {
            var testTrialBalanceLineItems = account.CreateSet<Transaction>();
            return testTrialBalanceLineItems;
        }

        public class Transaction : ITransaction {
            public DateTime TransactionDate { get; set; }
            public int AccountNo { get; set; }
            public string TransactionReference { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }
    }
}
