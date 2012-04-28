using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper {
    public interface IJournal {
        IEnumerable<IJournalEntry> Entries { get; }
    }
}