using System;

namespace Bookkeeper.Infrastructure.Interfaces
{
    public interface IJournalEntry    
    {
        DateTime TransactionDate { get; }
        int AccountNo { get; }
        string TransactionReference { get; }
        decimal DebitAmount { get; }
        decimal CreditAmount { get; }
    }
}