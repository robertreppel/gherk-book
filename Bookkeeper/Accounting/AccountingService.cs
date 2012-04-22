using System;
using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    public class AccountingService : IAccountingService
    {
        public static AccountingService SetUpAccounting()
        {
            var accountingService = new AccountingService();

            const int salesTaxOwingAccount = 3000;
            accountingService.CreateSalesTaxOwingAccount(salesTaxOwingAccount, "Sales Tax Owing");

            const int salesTaxPaidAccount = 3001;
            accountingService.CreateSalesTaxPaidAccount(salesTaxPaidAccount, "Sales Tax Paid");

            const int cashRegister = 1000;
            accountingService.CreateCashRegisterAccount(cashRegister, "Cash");
            return accountingService;
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
            var cashAccount = _generalLedger[_cashRegisterAccountNo];
            cashAccount.RecordTransaction(amount,transactionDate, transactionReference);

            var customerAccount = _generalLedger[customerAccountNo];
            customerAccount.RecordTransaction(amount, transactionDate, transactionReference);
        }

        public void RecordTaxableSale(int customerAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _generalLedger[_cashRegisterAccountNo];
            cashAccount.RecordTransaction(netAmount + salesTaxAmount, transactionDate, transactionReference);

            var customerAccount = _generalLedger[customerAccountNo];
            customerAccount.RecordTransaction(netAmount, transactionDate, transactionReference);

            var salesTaxOwingAccount = _generalLedger[_salesTaxOwingAccountNo];
            salesTaxOwingAccount.RecordTransaction(salesTaxAmount, transactionDate, transactionReference);
        }

        public void RecordPurchaseFrom(int supplierAccountNo, int assetAccountNo, decimal netAmount, decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            AddToSalesTaxPaid(salesTaxAmount, transactionDate, transactionReference);
            DeductFromSalesTaxOwing(salesTaxAmount, transactionDate, transactionReference);
            RecordAmountOwingTo(supplierAccountNo, netAmount, transactionDate, transactionReference);
            RecordAsset(assetAccountNo, netAmount, transactionDate, transactionReference);
        }

        public void PayTo(int recipientAccountNo, decimal amount, DateTime transactionDate, string transactionReference)
        {
            var cashAccount = _generalLedger[_cashRegisterAccountNo];
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
            var salesTaxPaidAccount = _generalLedger[_salesTaxPaidAccountNo];
            salesTaxPaidAccount.RecordTransaction(salesTaxAmount, transactionDate, transactionReference);
        }

        private void DeductFromSalesTaxOwing(decimal salesTaxAmount, DateTime transactionDate, string transactionReference)
        {
            var salesTaxOwingAccount = _generalLedger[_salesTaxOwingAccountNo];
            salesTaxOwingAccount.RecordTransaction((salesTaxAmount * -1), transactionDate, transactionReference);
        }

        private void CreateCashRegisterAccount(int cashRegister, string accountName)
        {
            _cashRegisterAccountNo = cashRegister;
            CreateNewAccount(cashRegister, accountName, AccountType.Asset);
        }

        private void CreateSalesTaxOwingAccount(int salesTaxAccountNo, string accountName)
        {
            _salesTaxOwingAccountNo = salesTaxAccountNo;
            CreateNewAccount(salesTaxAccountNo, accountName, AccountType.Liability);
        }

        private void CreateSalesTaxPaidAccount(int salesTaxPaidAccountNo, string accountName)
        {
            _salesTaxPaidAccountNo = salesTaxPaidAccountNo;
            CreateNewAccount(salesTaxPaidAccountNo, accountName, AccountType.Equity);
        }

        private AccountingService()
        {
        }

        private readonly Dictionary<int, Account> _generalLedger = new Dictionary<int, Account>();
        private readonly IJournal _journal = new Journal();
        private int _salesTaxOwingAccountNo;
        private int _cashRegisterAccountNo;
        private int _salesTaxPaidAccountNo;

        public IAccount GetStatementFor(int accountNo)
        {
            return _generalLedger[accountNo];
        }
    }

}
