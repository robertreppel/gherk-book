namespace Bookkeeper
{
    public interface IPrintGeneralLedgerReports
    {
        ISubLedger For { set; }

        void Print<T>();
        void Print<T>(int accountNumber);

    }
}