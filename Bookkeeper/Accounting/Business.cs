using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting {
    public class Business : IBusiness {
        public static IBusiness Create() {
            return new Business();
        }

        private Business() {}

        private readonly Dictionary<string, ILedger> _ledgers = new Dictionary<string, ILedger>();

        public void Add<T>(object accountingArtifact) where T: ILedger {
            if(accountingArtifact is ILedger) {
                var ledger = (ILedger) accountingArtifact;
                _ledgers.Add(ledger.Name, ledger);
            }
        }

        public T Find<T>(string key) where T: ILedger {
            var result = _ledgers[key];
            return (T) result;
        }

        public T Find<T>(int key) where T: IAccount {
            var ledger = (from l in _ledgers.Values
                          where (from a in l.Accounts where a.AccountNumber == key select a).Count() > 0
                          select l).FirstOrDefault();
            return (T) (from a in ledger.Accounts where a.AccountNumber == key select a).FirstOrDefault();
        }
    }
}