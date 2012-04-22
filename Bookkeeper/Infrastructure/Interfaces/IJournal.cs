using System.Collections.Generic;

namespace Bookkeeper.Infrastructure.Interfaces
{
    internal interface IJournal
    {
        void Add(IJournalEntry entry);
        IEnumerable<IJournalEntry> Entries();
        IEnumerable<IJournalEntry> EntriesFor(int accountNumber);
    }
}