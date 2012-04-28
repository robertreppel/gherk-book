using System.Collections.Generic;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper
{
    public interface IDoBookkeeping
    {
        void CreateNewAccount(int accountNumber, string accountName, AccountType type);

        IEnumerable<IAccount> GetChartOfAccounts();
        IEnumerable<IJournalEntry> GetJournal();
        ITrialBalance GetTrialBalance();
        IAccount GetAccount(int accountNo);
    }
}