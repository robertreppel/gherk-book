using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class TrialBalance : ITrialBalance
    {
        public bool IsBalanced { get; set; }

        internal TrialBalance(ISubLedger ledger) {
            var accounts = ledger.Accounts;
            GenerateFrom(accounts);
        }

        public IEnumerable<ITrialBalanceLineItem> LineItems { get; set; }
        public decimal TotalDebitAmount { get; private set; }
        public decimal TotalCreditAmount { get; private set; }

        private void GenerateFrom(IEnumerable<IAccount> accounts)
        {
            var lineItems = GenerateLineItemsFrom(accounts);

            var debitsGrandTotal = 0.0m;
            var creditsGrandTotal = 0.0m;
            foreach (var lineItem in lineItems)
            {
                debitsGrandTotal = debitsGrandTotal + lineItem.Debit;
                creditsGrandTotal = creditsGrandTotal + lineItem.Credit;
            }

            LineItems = lineItems;
            IsBalanced = IsEqual(debitsGrandTotal, creditsGrandTotal);
            TotalDebitAmount = debitsGrandTotal;
            TotalCreditAmount = creditsGrandTotal;
        }

        private static bool IsEqual(decimal debitsGrandTotal, decimal creditsGrandTotal)
        {
            return debitsGrandTotal == creditsGrandTotal;
        }

        private IEnumerable<ITrialBalanceLineItem> GenerateLineItemsFrom(IEnumerable<IAccount> accounts)
        {
            var lineItems = new List<ITrialBalanceLineItem>();
            foreach (var thisAccount in accounts)
            {
                lineItems.Add(LineItemWithTotalDebitsAndCreditsFor(thisAccount));
            }
            return lineItems;
        }

        private static TrialBalanceLineItem LineItemWithTotalDebitsAndCreditsFor(IAccount account)
        {
            var totalDebits = 0.0m;
            var totalCredits = 0.0m;
            foreach (var transaction in account.Transactions)
            {
                totalDebits = totalDebits + transaction.DebitAmount;
                totalCredits = totalCredits + transaction.CreditAmount;
            }
            return new TrialBalanceLineItem(account.AccountNumber, account.Name, totalDebits,
                                            totalCredits, account.Type);
        }

    }
}