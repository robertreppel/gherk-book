using System;

namespace Bookkeeper.Infrastructure.Interfaces
{
    public interface ITransaction    
    {
        DateTime TransactionDate { get; }
        int AccountNo { get; }
        string TransactionReference { get; }
        decimal DebitAmount { get; }
        decimal CreditAmount { get; }
    }
}