Feature: Record Bookkeeping Transactions

	- Create accounts
	- Record transactions in the ledger

Scenario: Create an account
	Given a Accounts Receivable subledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable
	Then the trial balance of the Accounts Receivable subledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit | Credit |
         |       1236    |   Revenue | Sales (Services)  |   0.0 |   0.0  |


Scenario: Record a transaction
	Given a Accounts Receivable subledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable

	When I record the following transaction in the Accounts Receivable ledger:
		| AccountNumber | AccountName      | Date      | TransactionReference		    | Amount    |
		| 1236          | Sales (Services) | 12/3/2011 | Consulting,  Harry Slayton     | 2034.12   |
		
	Then the trial balance of the Accounts Receivable subledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit	| Credit	|
         |       1236    |   Revenue | Sales (Services)  |  0.0		|   2034.12	|

	And the Accounts Receivable ledger should not balance.
