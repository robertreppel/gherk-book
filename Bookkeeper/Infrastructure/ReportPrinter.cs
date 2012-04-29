namespace Bookkeeper.Infrastructure
{
    public static class ReportPrinter
    {
        public static IPrintLedgerReports For(ILedger ledger)
        {
            var reportPrinter = Ioc.Resolve<IPrintLedgerReports>();
            reportPrinter.For = ledger;
            return reportPrinter;
        }
    }
}