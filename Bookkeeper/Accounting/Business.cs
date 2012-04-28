using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class Business : Bookkeeping, IAmAConsultingBusiness
    {
        const int SalesTaxOwing = 3002;
        const int CashRegister = 1000;
        const int OwnerEquity = 7000;

        public static Business SetUpAccounting() {
            var subsidiaryLedger = new SubsidiaryLedger();
            var accountingService = new Business(subsidiaryLedger);

            accountingService.CreateSalesTaxOwingAccount();
            accountingService.CreateCashRegisterAccount();
            accountingService.CreateOwnerEquityAccount();

            return accountingService;
        }

        public int CashRegisterAcctNo
        {
            get { return CashRegister; }
        }

        public int OwnersEquityAcctNo
        {
            get { return OwnerEquity; }
        }


        public int SalesTaxOwingAcctNo
        {
            get { return SalesTaxOwing; }
        }

 
        public void RecordTaxFreeSale(int customerAccountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = SubsidiaryLedger.GetAccount(customerAccountNo);
            cashAccount.RecordTransaction(amount,transactionDate, transactionReference);

            var customerAccount = _subsidiaryLedger.Accounts;
            customerAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }

        public void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _subsidiaryLedger.Accounts;
            cashAccount.RecordTransaction(netAmount + salesTaxAmount, transactionDate, transactionReference);

            var customerAccount = _subsidiaryLedger.Accounts;
            customerAccount.RecordTransaction(netAmount, transactionDate, transactionReference);

            var salesTaxOwingAccount = _subsidiaryLedger.Accounts;
            salesTaxOwingAccount.RecordTransaction(salesTaxAmount, transactionDate, transactionReference);
        }

        public void RecordPurchaseFrom(int supplierAccountNo, int assetAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            if(salesTaxAmount > 0)
            {
                DeductFromSalesTaxOwing(salesTaxAmount, transactionDate, transactionReference);
            }
            RecordAmountOwingTo(supplierAccountNo, netAmount + salesTaxAmount, transactionDate, transactionReference);
            RecordAsset(assetAccountNo, netAmount, transactionDate, transactionReference);
        }

        public void RecordPaymentTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _subsidiaryLedger.Accounts;
            cashAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);

            var recipientAccount = _subsidiaryLedger.Accounts;
            recipientAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);
        }

        private void RecordAsset(int assetAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var assetAccount = _subsidiaryLedger.Accounts;
            assetAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void RecordAmountOwingTo(int supplierAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var supplierAccount = _subsidiaryLedger.Accounts;
            supplierAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void DeductFromSalesTaxOwing(decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var salesTaxOwingAccount = _subsidiaryLedger.Accounts;
            salesTaxOwingAccount.RecordTransaction((salesTaxAmount * -1), transactionDate, transactionReference);
        }

        private void CreateCashRegisterAccount()
        {
            _subsidiaryLedger.CreateNewAccount(CashRegister, "Cash", AccountType.Asset);
        }

        private void CreateSalesTaxOwingAccount()
        {
            _bookkeeper.CreateNewAccount(SalesTaxOwing, "Sales Tax Owing", AccountType.Liability);
        }

        private void CreateOwnerEquityAccount()
        {
            _bookkeeper.CreateNewAccount(OwnerEquity, "John Smith", AccountType.Equity);
        }

        private Business(ISubsidiaryLedger subsidiaryLedger)
        {
            _subsidiaryLedger = subsidiaryLedger;
        }

        public void RecordCashInvestmentBy(int accountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            _subsidiaryLedger.Accounts.RecordTransaction(amount, transactionDate, transactionReference);
            var cashAccount = _subsidiaryLedger.Accounts;
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);

        }

        public void RecordCashInjectionByOwner(decimal amount, DateTime transactionDate, string transactionReference)
        {
            var ownerEquityAccount = _subsidiaryLedger.Accounts;
            ownerEquityAccount.RecordTransaction(amount, transactionDate, transactionReference);

            var cashAccount = _subsidiaryLedger.Accounts;
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }
    }
}
