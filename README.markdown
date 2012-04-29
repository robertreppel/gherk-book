Accounting Workbench
====================

__Bookkeeping__ must be one of the most standardized and well-documented domains in existence (http://en.wikipedia.org/wiki/Della_mercatura_e_del_mercante_perfetto).

This is an exercise I'm doing to understand the domain better. 
With the addition of SpecFlow steps it's beginning to look like a kind of "Accounting Workbench". 
It might be possible for developers to sit down with accountants and model the parts of applications which need to record
financial transactions:

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


It's rudimentary:

* There are ledgers.
* Accounts can be created.
* Transactions can be recorded.
* There is a simple report printer. It writes trial balances and account statements to the console.
* There is no user interface. Run the NUnit tests.
* There is no database.

__DISCLAIMER:__ I'm not an accountant. This was put together in a hurry, with domain knowledge googled on the fly. Stay out of jail.

