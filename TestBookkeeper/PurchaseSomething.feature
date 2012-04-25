Feature: Purchasing
	When I purchase something the supplier account should show the liability, the asset account
	should show the asset and sales tax eligible for government refund should be shown.

Scenario: Purchase office supplies
	Given a asset account 1234 "Office Supplies"
	And a liability account 5000 "Joe's Office Supplies"
	When I purchase Office Supplies (acct. 1234) for $4000 + $400 tax from "Joe's Office Supplies" (acct. 5000)
	Then the trial balance should look like this:

	|AccountNumber    |AcctType     |AccountName                       |     Debit    |    Credit|
	|1000             |Asset        |Cash                              |       0.0    |       0.0|
	|1234             |Asset        |Office Supplies                   |    4000.0    |       0.0|
	|3002             |Liability    |Sales Tax Owing                   |     400.0    |       0.0|
	|5000             |Liability    |Joe's Office Supplies             |       0.0    |    4000.0|
	|7000             |Equity       |John Smith (Owner)                |       0.0    |       0.0|
	|3001             |Revenue      |Sales Tax Refunds                 |       0.0    |     400.0|
	
	And the trial balance total should be $4400.
