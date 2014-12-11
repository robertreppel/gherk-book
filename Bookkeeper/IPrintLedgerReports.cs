namespace Bookkeeper
{
    public interface IPrintLedgerReports
    {
        ILedger For { set; }

        void Print<T>();
        void Print<T>(int accountNumber);
    }
}