using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class SubLedger : ISubLedger {
        public static SubLedger CreateSubLedger(int ledgerId, string ledgerName, int controllingAccountNumber, AccountType controllingAccountType) {
            return new SubLedger(ledgerId, ledgerName, controllingAccountNumber, controllingAccountType);
        }

        private readonly Dictionary<int, Account> _ledger;

        private SubLedger(int ledgerId, string ledgerName, int controllingAccountNumber, AccountType controllingAccountType) {
            _ledger = new Dictionary<int, Account>();
            Name = ledgerName;

            _controllingAccount = new Account(ledgerId, ledgerName, controllingAccountType);
        }

        private readonly Account _controllingAccount;

        public IAccount ControllingAccount {
            get { return _controllingAccount; }

        }

        public string Name { get; private set; }

        public ITrialBalance GetTrialBalance() {
            return new TrialBalance(this);
        }

        public IEnumerable<IAccount> Accounts {
            get { return _ledger.Values; }
        }

        public void AddAccount(int accountNo, string accountName, AccountType accountType) {
            var newAccount = new Account(accountNo, accountName, accountType);
            _ledger.Add(accountNo, newAccount);
        }

        public void RecordTransaction(int accountNumber, DateTime transactionDate, string transactionReference, decimal amount) {
            _ledger[accountNumber].RecordTransaction(amount, transactionDate, transactionReference);
        }
    }
}
