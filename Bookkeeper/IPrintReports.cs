namespace Bookkeeper
{
    public interface IPrintReports
    {
        IDoAccounting ForBusiness { set; }

        void Print<T>();
        void Print<T>(int id);

    }
}