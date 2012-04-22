Double Entry Bookkeeping Kata
=============================

__Bookkeeping__ must be one of the most standardized and well-documented domains in existence (http://en.wikipedia.org/wiki/Della_mercatura_e_del_mercante_perfetto).

The implementation maintains the accounting equation: 

__Assets = Liabilities + (Shareholders or Owners equity).__

* There is general ledger with a chart of accounts and a general journal.
* It's possible to post transactions.
* Trial balances can be generated.
* There are account statements (but the balance is not calculated - didn't get around to it.).
* There are no sales-, purchase- or other special journals. 

__The accounting bounded context of a fictional business__ is modeled in a service exposed by the IAccountingService interface:

* Cash sales only.
* Sales tax is handled like Canadian GST: Amount owing to the government = sales tax charged - sales tax paid.
* There is no inventory. Product for sale is represented by a transaction reference string. It's vague, ill-defined and therefore probably consulting services.

There is a report printer for trial balances and account statements. It's very dumb and writes to the console.

There is no user interface. Run the NUnit tests - they will Console.WriteLine trial balances and statements for the tests. (Try the "ShouldPayAccountsPayable" test.)

There is no database.

__DISCLAIMER:__ I'm not an accountant. This was put together in a hurry, with domain knowledge googled on the fly. Stay out of jail.
 

