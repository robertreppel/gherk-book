namespace Bookkeeper
{
    public interface IPrintLedgerReports
    {
        ILedger For { set; get; }

        void Print<T>();
        void Print<T>(int accountNumber);
    }
}