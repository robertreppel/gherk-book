using System;
using Bookkeeper;
using Bookkeeper.Accounting;
using TechTalk.SpecFlow;
using Business = Bookkeeper.Accounting.Business;

namespace TestBookkeeper
{
    [Binding]
    public class ConsultingBusinessStepDefinition
    {
        [Given(@"the following accounts: 3002 Sales Tax Owing, 1000 Cash, 3001 Sales Tax Paid, 7000 John Smith")]
        public void InitializeAccountingSystem()
        {
            var business = Business.SetUpAccounting();
            ScenarioContext.Current.Add("business", business);
        }



        [When(@"I purchase (.*) \(acct\. (\d+)\) for \$(\d+) \+ \$(\d+) tax from ""(.*)"" \(acct\. (\d+)\)")]
        public void WhenIMakeAPurchase(string assetAccountName, int assetAccountNo, decimal netAmount, decimal salesTax, string supplierName, int supplierAccountNo)
        {
            var business = (IAmAConsultingBusiness)ScenarioContext.Current["business"];
            business.RecordPurchaseFrom(supplierAccountNo, assetAccountNo, netAmount, salesTax, DateTime.Now, supplierName);
        }

 
    }
}
