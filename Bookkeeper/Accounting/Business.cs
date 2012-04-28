using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class Business : IAmAConsultingBusiness
    {
        private readonly IDoBookkeeping _bookkeeper;
        const int SalesTaxOwing = 3002;
        const int CashRegister = 1000;
        const int OwnerEquity = 7000;

        public static IAmAConsultingBusiness SetUpAccounting()
        {
            var bookkeeper = Ioc.Resolve<IDoBookkeeping>();
            var accountingService = new Business(bookkeeper);

            accountingService.CreateSalesTaxOwingAccount();
            accountingService.CreateCashRegisterAccount();
            accountingService.CreateOwnerEquityAccount();

            return accountingService;
        }

        public IDoBookkeeping Bookkeeper { get { return _bookkeeper; }}

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
            var cashAccount = _bookkeeper.GetAccount(CashRegister);
            cashAccount.RecordTransaction(amount,transactionDate, transactionReference);

            var customerAccount = _bookkeeper.GetAccount(customerAccountNo);
            customerAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }

        public void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _bookkeeper.GetAccount(CashRegister);
            cashAccount.RecordTransaction(netAmount + salesTaxAmount, transactionDate, transactionReference);

            var customerAccount = _bookkeeper.GetAccount(customerAccountNo);
            customerAccount.RecordTransaction(netAmount, transactionDate, transactionReference);

            var salesTaxOwingAccount = _bookkeeper.GetAccount(SalesTaxOwing);
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
            var cashAccount = _bookkeeper.GetAccount(CashRegister);
            cashAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);

            var recipientAccount = _bookkeeper.GetAccount(recipientAccountNo);
            recipientAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);
        }

        private void RecordAsset(int assetAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var assetAccount = _bookkeeper.GetAccount(assetAccountNo);
            assetAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void RecordAmountOwingTo(int supplierAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var supplierAccount = _bookkeeper.GetAccount(supplierAccountNo);
            supplierAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void DeductFromSalesTaxOwing(decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var salesTaxOwingAccount = _bookkeeper.GetAccount(SalesTaxOwing);
            salesTaxOwingAccount.RecordTransaction((salesTaxAmount * -1), transactionDate, transactionReference);
        }

        private void CreateCashRegisterAccount()
        {
            _bookkeeper.CreateNewAccount(CashRegister, "Cash", AccountType.Asset);
        }

        private void CreateSalesTaxOwingAccount()
        {
            _bookkeeper.CreateNewAccount(SalesTaxOwing, "Sales Tax Owing", AccountType.Liability);
        }

        private void CreateOwnerEquityAccount()
        {
            _bookkeeper.CreateNewAccount(OwnerEquity, "John Smith", AccountType.Equity);
        }

        private Business(IDoBookkeeping bookkeeper)
        {
            _bookkeeper = bookkeeper;
        }

        public void RecordCashInvestmentBy(int accountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            _bookkeeper.GetAccount(accountNo).RecordTransaction(amount, transactionDate, transactionReference);
            var cashAccount = _bookkeeper.GetAccount(CashRegisterAcctNo);
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);

        }

        public void RecordCashInjectionByOwner(decimal amount, DateTime transactionDate, string transactionReference)
        {
            var ownerEquityAccount = _bookkeeper.GetAccount(OwnersEquityAcctNo);
            ownerEquityAccount.RecordTransaction(amount, transactionDate, transactionReference);

            var cashAccount = _bookkeeper.GetAccount(CashRegisterAcctNo);
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }
    }
}
