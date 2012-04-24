using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class TrialBalance : ITrialBalance
    {
        public bool IsBalanced { get; private set; }

        private TrialBalance()
        {
        }

        public IEnumerable<ITrialBalanceLineItem> LineItems { get; private set; }
        public decimal TotalDebitAmount { get; private set; }
        public decimal TotalCreditAmount { get; private set; }

        public static ITrialBalance GenerateFrom(IEnumerable<IAccount> chartOfAccounts, IJournalRepository journal)
        {
            var lineItems = GenerateLineItemsFrom(chartOfAccounts, journal);

            var debitsGrandTotal = 0.0m;
            var creditsGrandTotal = 0.0m;
            foreach (var lineItem in lineItems)
            {
                debitsGrandTotal = debitsGrandTotal + lineItem.Debit;
                creditsGrandTotal = creditsGrandTotal + lineItem.Credit;
            }

            var trialBalance = new TrialBalance
                                   {
                                       LineItems = lineItems,
                                       IsBalanced = IsEqual(debitsGrandTotal, creditsGrandTotal),
                                       TotalDebitAmount = debitsGrandTotal,
                                       TotalCreditAmount = creditsGrandTotal
                                   };
            return trialBalance;
        }

        private static bool IsEqual(decimal debitsGrandTotal, decimal creditsGrandTotal)
        {
            return debitsGrandTotal == creditsGrandTotal;
        }

        private static IEnumerable<ITrialBalanceLineItem> GenerateLineItemsFrom(IEnumerable<IAccount> chartOfAccounts, IJournalRepository journal)
        {
            var lineItems = new List<ITrialBalanceLineItem>();
            foreach (var thisAccount in chartOfAccounts)
            {
                var journalEntriesFor = journal.EntriesFor(thisAccount.AccountNumber);
                lineItems.Add(LineItemWithTotalDebitsAndCreditsFor(journalEntriesFor, thisAccount));
            }
            return lineItems;
        }

        private static TrialBalanceLineItem LineItemWithTotalDebitsAndCreditsFor(IEnumerable<IJournalEntry> journalEntriesForAccount, IAccount account)
        {
            var totalDebits = 0.0m;
            var totalCredits = 0.0m;
            foreach (var journalEntry in journalEntriesForAccount)
            {
                totalDebits = totalDebits + journalEntry.DebitAmount;
                totalCredits = totalCredits + journalEntry.CreditAmount;
            }
            return new TrialBalanceLineItem(account.AccountNumber, account.Name, totalDebits,
                                            totalCredits, account.Type);
        }
    }
}