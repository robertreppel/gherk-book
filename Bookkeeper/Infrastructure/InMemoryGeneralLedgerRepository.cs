using System;
using System.Collections.Generic;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Infrastructure
{
    class InMemoryGeneralLedgerRepository : IGeneralLedgerRepository
    {
        private readonly Dictionary<int, Account> _generalLedger = new Dictionary<int, Account>();

        public IEnumerable<IAccount> Accounts()
        {
            return _generalLedger.Values;
        }

        public IAccount GetAccount(int accountNo)
        {
            return _generalLedger[accountNo];
        }

        public void AddAccount(int accountNumber, IAccount account)
        {
            if (!_generalLedger.ContainsKey(account.AccountNumber))
            {
                _generalLedger.Add(accountNumber, (Account) account);
            } else
            {
                throw new ArgumentException(String.Format("Account no. {0} already exists in the ledger.", accountNumber));
            }
        }
    }
}