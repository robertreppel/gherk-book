namespace Bookkeeper.Infrastructure
{
    public static class Ioc 
    {
        public static T Resolve<T>() where T : IPrintGeneralLedgerReports
        {
            return (T) (new ConsoleGeneralLedgerReportPrinter() as IPrintGeneralLedgerReports);
        }
    }
}