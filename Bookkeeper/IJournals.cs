using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper {
    public interface IJournals {
        IEnumerable<IJournalEntry> GetJournal();
    }
}