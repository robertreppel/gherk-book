namespace Bookkeeper.Infrastructure
{
    public static class ReportPrinter
    {
        public static IPrintReports For(IAccountingService accountingService)
        {
            return new ConsoleReportPrinter(accountingService);
        }
    }
}