using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;
using NUnit.Framework;
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

        private bool Matches(IEnumerable<ITransaction> expectedAccountTransactions, IEnumerable<ITransaction> actualTransactions) {
            throw new NotImplementedException();
        }

        [StepArgumentTransformation]
        public IEnumerable<ITransaction> AccountTransform(Table account)
        {
            var testTrialBalanceLineItems = account.CreateSet<Transaction>();
            return testTrialBalanceLineItems;
        }

        public class Transaction : ITransaction {
            public DateTime TransactionDate { get; private set; }
            public int AccountNo { get; private set; }
            public string TransactionReference { get; private set; }
            public decimal Debit { get; private set; }
            public decimal Credit { get; private set; }
        }
    }
}
