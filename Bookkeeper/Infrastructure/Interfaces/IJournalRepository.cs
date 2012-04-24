using System.Collections.Generic;

namespace Bookkeeper.Infrastructure.Interfaces
{
    internal interface IJournalRepository
    {
        void Add(IJournalEntry entry);
        IEnumerable<IJournalEntry> Entries();
        IEnumerable<IJournalEntry> EntriesFor(int accountNumber);
    }
}