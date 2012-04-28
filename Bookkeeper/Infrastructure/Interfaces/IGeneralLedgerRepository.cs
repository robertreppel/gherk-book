using System.Collections.Generic;

namespace Bookkeeper.Infrastructure.Interfaces
{
    internal interface IGeneralLedgerRepository
    {
        IEnumerable<IAccount> Accounts();
        IAccount GetAccount(int accountNo);
        void AddAccount(int accountNumber, IAccount account);
    }
}