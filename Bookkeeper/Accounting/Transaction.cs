using System;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class Transaction : ITransaction
    {
        public Transaction(DateTime transactionDate, string transactionReference, int accountNo, decimal debitAmount, decimal creditAmount)
        {
            TransactionDate = transactionDate;
            AccountNo = accountNo;
            TransactionReference = transactionReference;
            Debit = debitAmount;
            Credit = creditAmount;
        }

        public DateTime TransactionDate { get; private set; }
        public int AccountNo { get; private set; }
        public string TransactionReference { get; private set; }
        public decimal Debit { get; private set; }
        public decimal Credit { get; private set; }
    }
}