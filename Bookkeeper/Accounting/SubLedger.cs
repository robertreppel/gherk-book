using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    class SubLedger : ISubLedger {
        public ITrialBalance GetTrialBalance() {
            throw new NotImplementedException();
        }

        public Dictionary<int, IAccount> Accounts {
            get { throw new NotImplementedException(); }
        }

        public void AddAccount(Account account) {
            throw new NotImplementedException();
        }

        public Account GetAccount(int customerAccountNo) {
            throw new NotImplementedException();
        }
    }
}
