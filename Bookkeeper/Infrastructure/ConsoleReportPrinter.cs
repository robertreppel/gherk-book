using System;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Infrastructure
{
    internal class ConsoleReportPrinter : IPrintReports
    {
        private IDoAccounting _business;

        public IDoAccounting ForBusiness { set { _business = value; } }

        public void Print<T>()
        {
            var reportName = typeof(T).Name;
            if(reportName == "ITrialBalance")
            {
                PrintTrialBalance(_business.GetTrialBalance());
            } else if(reportName == "IAccount")
            {
                PrintStatementFor(_business.GetAccount(123));
            } else
            {
                throw new ReportingException("Unknown report: '" + reportName + "'.");
            }
        }

        public void Print<T>(int id)
        {
            var reportName = typeof(T).Name;
            if (reportName == "ITrialBalance")
            {
                PrintTrialBalance(_business.GetTrialBalance());
            }
            else if (reportName == "IAccount")
            {
                PrintStatementFor(_business.GetAccount(id));
            }
            else
            {
                throw new ReportingException("Unknown report: '" + reportName + "'.");
            }
        }


        private static void PrintStatementFor(IAccount account)
        {
            Console.WriteLine("Statement of Account - " + account.Name);
            Console.WriteLine("-------------------------------------------------------------------------");
            const string accountStatementFormat = "{0,-12}\t{1,-50}\t{2,10}\t{3,10}";
            Console.WriteLine(String.Format(accountStatementFormat, "Date", "Transaction", "Debit", "Credit"));

            var sortedLineItems = (from transaction in account.Transactions
                                   select transaction).OrderBy(x => x.TransactionDate);

            foreach (var lineItem in sortedLineItems)
            {
                Console.WriteLine(String.Format(accountStatementFormat, lineItem.TransactionDate.ToShortDateString(), 
                    lineItem.TransactionReference, lineItem.DebitAmount, lineItem.CreditAmount));
            }
            Console.WriteLine("Balance: " + account.Balance);
            Console.WriteLine();
        }

        public class ReportingException : Exception
        {
            public ReportingException(string errorMessage) : base(errorMessage)
            {
            }
        }

        private static void PrintTrialBalance(ITrialBalance trialBalance)
        {
            Console.WriteLine("Trial Balance");
            Console.WriteLine("-------------");
            const string formatProvider = "|{0,-13}\t|{1,-12}\t|{2,-30}\t|{3,10}\t|{4,10}|";
            Console.WriteLine(String.Format(formatProvider, "AccountNumber", "AcctType", "AccountName", "Debit", "Credit"));

            var sortedLineItems = (from ln in trialBalance.LineItems
                                   select ln).OrderBy(x => x.AcctType);

            foreach (var lineItem in sortedLineItems)
            {
                Console.WriteLine(String.Format(formatProvider, lineItem.AccountNumber, lineItem.AcctType, lineItem.AccountName, lineItem.Debit, lineItem.Credit));
            }
            Console.WriteLine(String.Format(formatProvider, "Totals:", "", "", trialBalance.TotalDebitAmount, trialBalance.TotalCreditAmount));
            Console.WriteLine();
        }
    }
}