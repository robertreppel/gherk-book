using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class Ledger : ILedger {
        public static Ledger CreateLedger(int ledgerId, string ledgerName) {
            return new Ledger(ledgerName);
        }

        private readonly Dictionary<int, Account> _ledger;

        private Ledger(string ledgerName) {
            _ledger = new Dictionary<int, Account>();
            Name = ledgerName;
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
