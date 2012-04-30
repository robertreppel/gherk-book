using TechTalk.SpecFlow;

namespace TestBookkeeper
{
    [Binding]
    public class AccountValidationStepDefinition
    {
        [Then(@"the account ""Sales \(Services\)"" contains the following:")]
        public void ThenTheAccountSalesServicesContainsTheFollowing(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
