Feature: Record Bookkeeping Transactions

	- Create accounts
	- Record transactions in the ledger

Scenario: Create an account
	Given a business
	And a revenue account 1236 "Sales (Services)"
	Then the trial balance should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit | Credit |
         |       1236    |   Revenue | Sales (Services)  |   0.0 |   0.0  |
