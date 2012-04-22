using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper.Accounting
{
    internal class Journal : IJournal
    {
        readonly List<IJournalEntry> _journalEntries = new List<IJournalEntry>();

        public void Add(IJournalEntry entry)
        {
            _journalEntries.Add(entry);
        }

        public IEnumerable<IJournalEntry> Entries()
        {
            return _journalEntries;
        }

        public IEnumerable<IJournalEntry> EntriesFor(int accountNumber)
        {
            var journalEntriesForAccount = (from entry in Entries()
                                            where entry.AccountNo == accountNumber
                                            select entry).ToList();
            return journalEntriesForAccount;
        }
    }
}