using Bookkeeper;
using Bookkeeper.Accounting;
using TechTalk.SpecFlow;

namespace TestBookkeeper {
    [Binding]
    public class BookkeepingStepDefinitions {

        [Given(@"a (.*) subledger with controlling account no. (\d+")]
        public void GivenASubledger(string ledgerName, int controllingAccountNumber)
        {
            if (!ScenarioContext.Current.ContainsKey("bookkeeper")) {
                var bookkeeper = Bookkeeping.Create();
                ScenarioContext.Current.Add("bookkeeper", bookkeeper);
            }
        }

        [Given(@"a (asset|liability|revenue|expense|equity) account (\d+) ""(.*)""")]
        public void CreateNewAccount(AccountType accountType, int accountNo, string accountName) {

            Bookkeeper().CreateNewAccount(accountNo, accountName, accountType);
        }

        private static IDoBookkeeping Bookkeeper() {

            return (IDoBookkeeping) ScenarioContext.Current["bookkeeper"];
        }
    }
}