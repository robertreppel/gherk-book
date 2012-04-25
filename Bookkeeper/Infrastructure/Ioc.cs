using System;
using System.Runtime.CompilerServices;
using Bookkeeper.Infrastructure.Interfaces;

[assembly: InternalsVisibleTo("TestBookkeeper")]
namespace Bookkeeper.Infrastructure
{
    public static class Ioc 
    {
        public static T Resolve<T>()
        {
            var typeName = typeof(T).Name;
            if (typeName == "IJournalRepository")
            {
                return (T) (new InMemoryJournalRepository() as IJournalRepository);
            }
            if(typeName == "IGeneralLedgerRepository")
            {
                return (T) (new InMemoryGeneralLedgerRepository() as IGeneralLedgerRepository);
            }
            if(typeName == "IPrintReports")
            {
                return (T) (new ConsoleReportPrinter() as IPrintReports);
            }
            throw new TypeLoadException(String.Format("Cannot resolve the type '{0}'", typeName));
        }
    }
}