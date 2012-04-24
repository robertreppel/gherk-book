using System;
using System.Collections.Generic;
using Bookkeeper.Accounting;

namespace Bookkeeper.Infrastructure.Interfaces
{
    public interface IAccount
    {
        int AccountNumber { get; }
        string Name { get; set; }
        AccountType Type { get; }
        IEnumerable<IJournalEntry> Transactions { get; }
        decimal Balance { get; }
        void RecordTransaction(decimal amount, DateTime transactionDate, string transactionReference);
    }
}