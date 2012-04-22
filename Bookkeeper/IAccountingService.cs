using System;
using System.Collections.Generic;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper
{
    public interface IAccountingService
    {
        void CreateNewAccount(int accountNumber, string accountName, AccountType type);

        void RecordTaxFreeSale(int customerAccountNo, decimal amount, DateTime transactionDate, string transactionReference);

        void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference);

        void RecordPurchaseFrom(int supplierAccountNo, int assetAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference);

        void PayTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference);

        IEnumerable<IAccount> GetChartOfAccounts();
        IEnumerable<IJournalEntry> GetJournal();
        ITrialBalance GetTrialBalance();
        IAccount GetAccount(int accountNo);
    }
}