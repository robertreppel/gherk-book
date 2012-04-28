using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper
{
    public interface IDoBookkeeping
    {
        void CreateNewAccount(int accountNumber, string accountName, AccountType type);
        ITrialBalance GetTrialBalance();
        ISubLedger SubLedger { get; }
    }
}