using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class AccountingService : IAccountingService
    {
        private readonly Dictionary<int, Account> _generalLedger = new Dictionary<int, Account>();
        private readonly IJournal _journal = new Journal();
        const int SalesTaxOwing = 3002;
        const int CashRegister = 1000;
        const int SalesTaxPaid = 3001;
        const int OwnerEquity = 7000;

        public static IAccountingService SetUpAccounting()
        {
            var accountingService = new AccountingService();

            accountingService.CreateSalesTaxOwingAccount();

            accountingService.CreateSalesTaxPaidAccount();

            accountingService.CreateCashRegisterAccount();

            accountingService.CreateOwnerEquityAccount();

            return accountingService;
        }

        public int SalesTaxPaidAcctNo
        {
            get { return SalesTaxPaid; }
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

        public IEnumerable<IJournalEntry> GetJournal()
        {
            return _journal.Entries();
        }

        public ITrialBalance GetTrialBalance()
        {
            return TrialBalance.GenerateFrom(_generalLedger, _journal);
        }

        public IAccount GetAccount(int accountNo)
        {
            return _generalLedger[accountNo];
        }

        public void CreateNewAccount(int accountNumber, string accountName, AccountType type)
        {
            var account = new Account(accountNumber, accountName, type);
            account.Journal = _journal;
            _generalLedger.Add(accountNumber, account);
        }

        public void RecordTaxFreeSale(int customerAccountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _generalLedger[CashRegister];
            cashAccount.RecordTransaction(amount,transactionDate, transactionReference);

            var customerAccount = _generalLedger[customerAccountNo];
            customerAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }

        public void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _generalLedger[CashRegister];
            cashAccount.RecordTransaction(netAmount + salesTaxAmount, transactionDate, transactionReference);

            var customerAccount = _generalLedger[customerAccountNo];
            customerAccount.RecordTransaction(netAmount, transactionDate, transactionReference);

            var salesTaxOwingAccount = _generalLedger[SalesTaxOwing];
            salesTaxOwingAccount.RecordTransaction(salesTaxAmount, transactionDate, transactionReference);
        }

        public void RecordPurchaseFrom(int supplierAccountNo, int assetAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            if(salesTaxAmount > 0)
            {
                AddToSalesTaxPaid(salesTaxAmount, transactionDate, transactionReference);
                DeductFromSalesTaxOwing(salesTaxAmount, transactionDate, transactionReference);
            }
            RecordAmountOwingTo(supplierAccountNo, netAmount, transactionDate, transactionReference);
            RecordAsset(assetAccountNo, netAmount, transactionDate, transactionReference);
        }

        public void RecordPaymentTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _generalLedger[CashRegister];
            cashAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);

            var recipientAccount = _generalLedger[recipientAccountNo];
            recipientAccount.RecordTransaction((amount * -1), transactionDate, transactionReference);
        }

        public IEnumerable<IAccount> GetChartOfAccounts()
        {
            return _generalLedger.Values;
        }


        private void RecordAsset(int assetAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var assetAccount = _generalLedger[assetAccountNo];
            assetAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void RecordAmountOwingTo(int supplierAccountNo, decimal netAmount, DateTime transactionDate, string transactionReference)
        {
            var supplierAccount = _generalLedger[supplierAccountNo];
            supplierAccount.RecordTransaction(netAmount, transactionDate, transactionReference);
        }

        private void AddToSalesTaxPaid(decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var salesTaxPaidAccount = _generalLedger[SalesTaxPaid];
            salesTaxPaidAccount.RecordTransaction(salesTaxAmount, transactionDate, transactionReference);
        }

        private void DeductFromSalesTaxOwing(decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var salesTaxOwingAccount = _generalLedger[SalesTaxOwing];
            salesTaxOwingAccount.RecordTransaction((salesTaxAmount * -1), transactionDate, transactionReference);
        }

        private void CreateCashRegisterAccount()
        {
            CreateNewAccount(CashRegister, "Cash", AccountType.Asset);
        }

        private void CreateSalesTaxOwingAccount()
        {
            CreateNewAccount(SalesTaxOwing, "Sales Tax Owing", AccountType.Liability);
        }

        private void CreateSalesTaxPaidAccount()
        {
            CreateNewAccount(SalesTaxPaid, "Sales Tax Refunds", AccountType.Revenue);
        }

        private void CreateOwnerEquityAccount()
        {
            CreateNewAccount(OwnerEquity, "John Smith (Owner)", AccountType.Equity);
        }

        private AccountingService()
        {
        }

        public IAccount GetStatementFor(int accountNo)
        {
            return _generalLedger[accountNo];
        }

        public void RecordCashInvestmentBy(int accountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            _generalLedger[accountNo].RecordTransaction(amount,transactionDate, transactionReference);
            var cashAccount = _generalLedger[CashRegisterAcctNo];
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);

        }

        public void RecordCashInjectionByOwner(decimal amount, DateTime transactionDate, string transactionReference)
        {
            var ownerEquityAccount = _generalLedger[OwnersEquityAcctNo];
            ownerEquityAccount.RecordTransaction(amount, transactionDate, transactionReference);

            var cashAccount = _generalLedger[CashRegisterAcctNo];
            cashAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }
    }

}
