using System;
using Bookkeeper;
using Bookkeeper.Accounting;
using TechTalk.SpecFlow;

namespace TestBookkeeper
{
    [Binding]
    public class BusinessAccountingStepDefinition
    {
        [BeforeScenario]
        public void InitializeAccountingSystem()
        {
            var business = Business.SetUpAccounting();
            ScenarioContext.Current.Add("business", business);
        }

        [Given(@"a (asset|liability|revenue|expense|equity) account (\d+) ""(.*)""")]
        public void CreateNewAccount(string accountDescription, int accountNo, string accountName)
        {
            var business = (IDoAccounting)ScenarioContext.Current["business"];
            business.CreateNewAccount(accountNo, accountName, AccountTypeFrom(accountDescription));
        }

        [When(@"I purchase (.*) \(acct\. (\d+)\) for \$(\d+) \+ \$(\d+) tax from ""(.*)"" \(acct\. (\d+)\)")]
        public void WhenIMakeAPurchase(string assetAccountName, int assetAccountNo, decimal netAmount, decimal salesTax, string supplierName, int supplierAccountNo)
        {
            var business = (IDoAccounting)ScenarioContext.Current["business"];
            business.RecordPurchaseFrom(supplierAccountNo, assetAccountNo, netAmount, salesTax, DateTime.Now, supplierName);
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
