namespace Bookkeeper.Infrastructure
{
    public static class ReportPrinter
    {
        public static IPrintReports For(IDoBookkeeping bookkeeper)
        {
            var reportPrinter = Ioc.Resolve<IPrintReports>();
            reportPrinter.For = bookkeeper;
            return reportPrinter;
        }
    }
}