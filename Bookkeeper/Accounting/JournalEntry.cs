using System;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class JournalEntry : IJournalEntry
    {
        public JournalEntry(DateTime transactionDate, string transactionReference, int accountNo, decimal debitAmount, decimal creditAmount)
        {
            TransactionDate = transactionDate;
            AccountNo = accountNo;
            TransactionReference = transactionReference;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
        }

        public DateTime TransactionDate { get; private set; }
        public int AccountNo { get; private set; }
        public string TransactionReference { get; private set; }
        public decimal DebitAmount { get; private set; }
        public decimal CreditAmount { get; private set; }
    }
}