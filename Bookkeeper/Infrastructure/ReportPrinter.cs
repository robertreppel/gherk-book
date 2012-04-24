namespace Bookkeeper.Infrastructure
{
    public static class ReportPrinter
    {
        public static IPrintReports For(IDoAccounting business)
        {
            var reportPrinter = Ioc.Resolve<IPrintReports>();
            reportPrinter.ForBusiness = business;
            return reportPrinter;
        }
    }
}