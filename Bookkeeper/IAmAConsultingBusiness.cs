using System;
using Bookkeeper.Accounting;

namespace Bookkeeper
{
    public interface IAmAConsultingBusiness
    {

        void RecordTaxFreeSale(int customerAccountNo, decimal amount, DateTime transactionDate, string transactionReference);

        void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference);

        void RecordPurchaseFrom(int supplierAccountNo, int assetAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference);

        void RecordPaymentTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference);

        void RecordCashInvestmentBy(int accountNo, decimal amount, DateTime transactionDate,
                                    string transactionReference);

        void RecordCashInjectionByOwner(decimal amount, DateTime transactionDate, string transactionReference);

        int SalesTaxOwingAcctNo { get;  }
        int CashRegisterAcctNo { get;  }
        int OwnersEquityAcctNo { get; }
        IDoBookkeeping Bookkeeper { get; }
    }
}