using Bookkeeper.Accounting;

namespace Bookkeeper.Infrastructure.Interfaces
{
    public interface ITrialBalanceLineItem
    {
        int AccountNumber { get; }
        string AccountName { get; }
        AccountType AcctType { get;  }
        decimal Debit { get; }
        decimal Credit { get; }
    }
}