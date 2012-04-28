using Bookkeeper;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using TechTalk.SpecFlow;

namespace TestBookkeeper {
    [Binding]
    public class BookkeepingStepDefinitions {

        [Given(@"a (.*) subledger with id (\d+) and a (.*) account no. (\d+) as controlling account")]
        public void GivenASubledger(string ledgerName, int ledgerId, AccountType controllingAccountType, int controllingAccountNumber)
        {
            if (!ScenarioContext.Current.ContainsKey("subledger")) {
                var subledger = SubLedger.CreateSubLedger(ledgerId, ledgerName, controllingAccountNumber,
                                                          controllingAccountType);
                ScenarioContext.Current.Add("subledger", subledger);
            }
        }

        [Given(@"a (asset|liability|revenue|expense|equity) account (\d+) ""(.*)""")]
        public void CreateNewAccount(AccountType accountType, int accountNo, string accountName) {
            var ledger = (ISubLedger) ScenarioContext.Current["subledger"];
            ledger.AddAccount(accountNo, accountName, accountType);
        }
    }
}