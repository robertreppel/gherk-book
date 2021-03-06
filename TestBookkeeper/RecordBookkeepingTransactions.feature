﻿Feature: Record Bookkeeping Transactions

	- Create accounts
	- Record transactions in the ledger

Scenario: Create an account
	Given a Accounts Receivable ledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable
	Then the trial balance of the Accounts Receivable ledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit | Credit |
         |       1236    |   Revenue | Sales (Services)  |   0.0 |   0.0  |


Scenario: Record a transaction
	Given a Accounts Receivable ledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable

	When I record the following transaction in the Accounts Receivable ledger:
		| AccountNumber | AccountName      | Date      | TransactionReference		   | Amount    |
		| 1236          | Sales (Services) | 12/3/2011 | 5434 - Widgets, Harry Slayton | 2034.12   |
		
	Then the trial balance of the Accounts Receivable ledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit	| Credit	|
         |       1236    |   Revenue | Sales (Services)  |  0.0		|   2034.12	|

	And the Accounts Receivable ledger should not balance.

	And the "Sales (Services)" 1236 account contains the following:

	|         | TransactionDate | TransactionReference          | Debit   | Credit  |
	|		  | 12/3/2011       | 5434 - Widgets, Harry Slayton | 0.0     | 2034.12 |
	| Balance |			        |                               | 2034.12 |         |


Scenario: Record two transactions
	Given a Accounts Receivable ledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable

	And a Assets ledger with id 4001 and a asset account no. 4000 as controlling account
	And a asset account 2000 "Bank" in Assets


	When I record the following transaction in the Accounts Receivable ledger:
		| AccountNumber | AccountName      | Date      | TransactionReference		    | Amount    |
		| 1236          | Sales (Services) | 12/3/2011 | 3212 - Maintenance, Bill Jones | 2034.12   |

	And I record the following transaction in the Assets ledger:
		| AccountNumber | AccountName      | Date      | TransactionReference		     | Amount    |
		| 2000          | Bank			   | 12/3/2011 | 3212 - Maintenance, Bill Jones  | 2034.12   |
		

	Then the trial balance of the Accounts Receivable ledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit	| Credit	|
         |       1236    |   Revenue | Sales (Services)  | 0.0		|   2034.12	|
	And the Accounts Receivable ledger should not balance.

	And the trial balance of the Assets ledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit	| Credit	|
         |       2000    |   Asset   | Bank			     | 2034.12	|   0.0		|
	And the Assets ledger should not balance.
	
