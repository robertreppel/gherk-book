namespace Bookkeeper.Infrastructure
{
    public static class Ioc 
    {
        public static T Resolve<T>() where T : IPrintLedgerReports
        {
            return (T) (new ConsoleLedgerReportPrinter() as IPrintLedgerReports);
        }
    }
}