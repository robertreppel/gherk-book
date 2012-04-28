using System;
using Bookkeeper.Infrastructure.Interfaces;

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
            if (typeName == "IDoBookkeeping")
            {
                return (T)(new Bookkeep(Ioc.Resolve<IJournalRepository>(), Ioc.Resolve<IGeneralLedgerRepository>()) as IDoBookkeeping);
            }
            throw new TypeLoadException(String.Format("Cannot resolve the type '{0}'", typeName));
        }
    }
}