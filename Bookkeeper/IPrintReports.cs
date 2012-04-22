namespace Bookkeeper
{
    public interface IPrintReports
    {
        void Print<T>();
        void Print<T>(int id);
    }
}