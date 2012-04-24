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

        void RecordPaymentTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference);

        void RecordCashInvestmentBy(int accountNo, decimal amount, DateTime transactionDate,
                                    string transactionReference);

        void RecordCashInjectionByOwner(decimal amount, DateTime transactionDate, string transactionReference);


        IEnumerable<IAccount> GetChartOfAccounts();
        IEnumerable<IJournalEntry> GetJournal();
        ITrialBalance GetTrialBalance();
        IAccount GetAccount(int accountNo);

        int SalesTaxOwingAcctNo { get;  }
        int SalesTaxPaidAcctNo { get;  }
        int CashRegisterAcctNo { get;  }
        int OwnersEquityAcctNo { get; }
    }
}