using System;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class Account : IAccount
    {
        private IJournal _journal;
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

        public Account(int accountNumber, string name, AccountType accountAccountType)
        {
            AccountNumber = accountNumber;
            Name = name;
            Type = accountAccountType;
            _journal = null;
        }

        public IJournal Journal { set { _journal = value; } }

        protected internal void RecordTransaction(decimal amount, DateTime transactionDate, string transactionReference)
        {
            if (amount == 0) throw new AccountException("Cannot record a transaction with a zero amount.");
            IJournalEntry journalEntry = null;
            switch (Type)
            {
                case AccountType.Asset:
                    if(amount > 0)
                    {
                        journalEntry = Debit(transactionDate, transactionReference, amount);
                    } else
                    {
                        journalEntry = Credit(transactionDate, amount, transactionReference);                        
                    }                    
                    break;
                case AccountType.Liability:
                    if(amount > 0)
                    {
                        journalEntry = Credit(transactionDate, amount, transactionReference);
                    } else
                    {
                        journalEntry = Debit(transactionDate, transactionReference, amount);                        
                    }                    
                    break;
                case AccountType.Revenue:
                    if(amount > 0)
                    {
                        journalEntry = Credit(transactionDate, amount, transactionReference);
                    } else
                    {
                        journalEntry = Debit(transactionDate, transactionReference, amount);                        
                    }                    
                    break;
                case AccountType.Expense:
                    if(amount > 0)
                    {
                        journalEntry = Debit(transactionDate, transactionReference, amount);
                    } else
                    {
                        journalEntry = Credit(transactionDate, amount, transactionReference);                        
                    }
                    break;
                case AccountType.Equity:
                    if(amount > 0)
                    {
                        journalEntry = Credit(transactionDate, amount, transactionReference);
                    } else
                    {
                        journalEntry = Debit(transactionDate, transactionReference, amount);                        
                    }
                    break;
            }
            _journal.Add(journalEntry);                        
        }

        internal class AccountException : Exception
        {
            public AccountException(string errorMessage) : base(errorMessage)
            {
            }
        }

        private IJournalEntry Debit(DateTime transactionDate, string transactionReference, decimal amount)
        {
            return new JournalEntry(transactionDate, transactionReference, AccountNumber, Math.Abs(amount), 0.0m);
        }

        private JournalEntry Credit(DateTime transactionDate, decimal amount, string transactionReference)
        {
            return new JournalEntry(transactionDate, transactionReference, AccountNumber, 0.0m, Math.Abs(amount));
        }
    }
}