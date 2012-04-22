using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class TrialBalanceLineItem : ITrialBalanceLineItem
    {
        public int AccountNumber { get; private set; }
        public string AccountName { get; private set; }

        public AccountType AcctType { get; private set; }

        public decimal Debit { get; private set; }
        public decimal Credit { get; private set; }

        public TrialBalanceLineItem(int accountNumber, string accountName, decimal debit, decimal credit, AccountType type)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
            Debit = debit;
            Credit = credit;
            AcctType = type;
        }
    }
}