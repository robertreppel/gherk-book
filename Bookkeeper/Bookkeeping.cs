using System;
using System.Collections.Generic;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper
{
    public class Bookkeeping : IDoBookkeeping
    {
        private readonly SubLedger _subLedger;

        internal Bookkeeping(IJournal journal, ISubLedger subLedger)
        {
            _subLedger = (SubLedger) subLedger;
        }

        public void CreateNewAccount(int accountNumber, string accountName, AccountType type)
        {
            var account = new Account(accountNumber, accountName, type);
            _subLedger.AddAccount(account);
        }

        public ITrialBalance GetTrialBalance() {
            return _subLedger.GetTrialBalance();
        }

        public ISubLedger SubLedger {
            get { return _subLedger; }
        }
    }

    internal class GeneralJournal : IJournal {
        public IEnumerable<IJournalEntry> Entries {
            get { throw new NotImplementedException(); }
        }
    }
}