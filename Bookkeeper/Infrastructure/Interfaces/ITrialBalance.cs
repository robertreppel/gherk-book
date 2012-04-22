using System.Collections.Generic;

namespace Bookkeeper.Infrastructure.Interfaces
{
    public interface ITrialBalance
    {
        bool IsBalanced { get; }
        IEnumerable<ITrialBalanceLineItem> LineItems { get; }
        decimal TotalDebitAmount { get; }
        decimal TotalCreditAmount { get; }
    }
}