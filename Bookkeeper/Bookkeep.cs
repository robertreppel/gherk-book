using System.Collections.Generic;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper
{
    public class Bookkeep : IDoBookkeeping
    {
        private readonly IGeneralLedgerRepository _generalLedger;
        private readonly IJournalRepository _journal;

        public static IDoBookkeeping Create() {
            return new Bookkeep(Ioc.Resolve<IJournalRepository>(), Ioc.Resolve<IGeneralLedgerRepository>());
        }

        internal Bookkeep(IJournalRepository journal, IGeneralLedgerRepository generalLedger)
        {
            _journal = journal;
            _generalLedger = generalLedger;
        }

        public void CreateNewAccount(int accountNumber, string accountName, AccountType type)
        {
            var account = new Account(accountNumber, accountName, type);
            account.Journal = _journal;
            _generalLedger.AddAccount(accountNumber, account);
        }

        public IEnumerable<IJournalEntry> GetJournal()
        {
            return _journal.Entries();
        }

        public ITrialBalance GetTrialBalance()
        {
            return TrialBalance.GenerateFrom(_generalLedger.GetAccounts(), _journal);
        }

        public IAccount GetAccount(int accountNo)
        {
            return _generalLedger.GetAccount(accountNo);
        }

        public IAccount GetStatementFor(int accountNo)
        {
            return _generalLedger.GetAccount(accountNo);
        }

        public IEnumerable<IAccount> GetChartOfAccounts()
        {
            return _generalLedger.GetAccounts();
        }

    }
}