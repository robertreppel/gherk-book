namespace Bookkeeper.Infrastructure
{
    public static class ReportPrinter
    {
        public static IPrintGeneralLedgerReports For(ISubLedger subLedger)
        {
            var reportPrinter = Ioc.Resolve<IPrintGeneralLedgerReports>();
            reportPrinter.For = subLedger;
            return reportPrinter;
        }
    }
}