namespace Bookkeeper
{
    public interface IPrintReports
    {
        IDoBookkeeping For { set; }

        void Print<T>();
        void Print<T>(int id);

    }
}