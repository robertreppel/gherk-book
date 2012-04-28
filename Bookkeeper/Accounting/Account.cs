using System;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class Account : IAccount
    {
        private IJournalRepository _journal;
        public int AccountNumber { get; private set; }
        public string Name { get; set; }
        public AccountType Type { get; private set; }

        public IEnumerable<IJournalEntry> Transactions 
        { get
        {
            var transactions = _journal.EntriesFor(AccountNumber);
                return transactions;
            } 
        }

        public decimal Balance
        {
             get
             {
                 var debits = (from e in _journal.EntriesFor(AccountNumber)
                               select e.DebitAmount).Sum();
                 var credits = (from e in _journal.EntriesFor(AccountNumber)
                               select e.CreditAmount).Sum();

                 var balance = debits - credits;

                 //Rules from http://en.wikipedia.org/wiki/Debits_and_credits:
                 switch(Type)
                 {
                     case(AccountType.Asset):
                         return DebitsIncreaseThe(balance);
                     case (AccountType.Liability):
                         return CreditsIncreaseThe(balance);
                     case (AccountType.Revenue):
                         return CreditsIncreaseThe(balance);
                     case (AccountType.Expense):
                         throw new NotImplementedException("Expense acct type balance not implemented.");
                         return 0;
                     case (AccountType.Equity):
                         return CreditsIncreaseThe(balance);
                 }
                 return 0;
             }
        }

        public void RecordTransaction(decimal amount, DateTime transactionDate, string transactionReference)
        {
            if (amount == 0) throw new Account.AccountException("Cannot record a transaction with a zero amount.");
            IJournalEntry journalEntry = null;

            switch (Type)
            {
                case AccountType.Asset:
                    if (amount > 0)
                    {
                        journalEntry = Debit(amount, transactionDate, transactionReference);
                    }
                    else
                    {
                        journalEntry = Credit(amount, transactionDate, transactionReference);
                    }
                    break;
                case AccountType.Liability:
                    if (amount > 0)
                    {
                        journalEntry = Credit(amount, transactionDate, transactionReference);
                    }
                    else
                    {
                        journalEntry = Debit(amount, transactionDate, transactionReference);
                    }
                    break;
                case AccountType.Revenue:
                    if (amount > 0)
                    {
                        journalEntry = Credit(amount, transactionDate, transactionReference);
                    }
                    else
                    {
                        journalEntry = Debit(amount, transactionDate, transactionReference);
                    }
                    break;
                case AccountType.Expense:
                    if (amount > 0)
                    {
                        journalEntry = Debit(amount, transactionDate, transactionReference);
                    }
                    else
                    {
                        journalEntry = Credit(amount, transactionDate, transactionReference);
                    }
                    break;
                case AccountType.Equity:
                    if (amount > 0)
                    {
                        journalEntry = Credit(amount, transactionDate, transactionReference);
                    }
                    else
                    {
                        journalEntry = Debit(amount, transactionDate, transactionReference);
                    }
                    break;
            }
            _journal.Add(journalEntry);
        }

        private IJournalEntry Debit(decimal amount, DateTime transactionDate, string transactionReference)
        {
            return new JournalEntry(transactionDate, transactionReference, AccountNumber, Math.Abs(amount), 0.0m);
        }

        private JournalEntry Credit(decimal amount, DateTime transactionDate, string transactionReference)
        {
            return new JournalEntry(transactionDate, transactionReference, AccountNumber, 0.0m, Math.Abs(amount));
        }

        private static decimal DebitsIncreaseThe(decimal balance)
        {
            return balance;
        }

        private static decimal CreditsIncreaseThe(decimal balance)
        {
            return balance * (-1);
        }


        public Account(int accountNumber, string name, AccountType accountAccountType)
        {
            AccountNumber = accountNumber;
            Name = name;
            Type = accountAccountType;
            _journal = null;
        }

        public IJournalRepository Journal { set { _journal = value; } }



        internal class AccountException : Exception
        {
            public AccountException(string errorMessage) : base(errorMessage)
            {
            }
        }


    }
}