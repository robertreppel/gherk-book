Accounting Workbench
====================

__Bookkeeping__ must be one of the most standardized and well-documented domains in existence (http://en.wikipedia.org/wiki/Della_mercatura_e_del_mercante_perfetto).

This is an exercise I'm doing to understand the domain better. 
With the addition of SpecFlow steps it's beginning to look like a kind of "Accounting Workbench". 
It might just be possible for developers to sit down with accountants and model the parts of applications which need to record
financial transactions:

Scenario: Create an account
	Given a Accounts Receivable subledger with id 3001 and a revenue account no. 3000 as controlling account
	And a revenue account 1236 "Sales (Services)" in Accounts Receivable
	Then the trial balance of the Accounts Receivable subledger should look like this:
         | AccountNumber | AcctType  | AccountName		 | Debit | Credit |
         |       1236    |   Revenue | Sales (Services)  |   0.0 |   0.0  |

It's still quite rudimentary:

* There are ledgers.
* There is a simple report printer. It writes trial balances and account statements to the console.
* There is no user interface. Run the NUnit tests.
* There is no database.

__DISCLAIMER:__ I'm not an accountant. This was put together in a hurry, with domain knowledge googled on the fly. Stay out of jail.

