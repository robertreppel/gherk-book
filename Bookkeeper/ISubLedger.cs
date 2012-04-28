using System.Collections.Generic;
using Bookkeeper.Infrastructure.Interfaces;

namespace Bookkeeper {
    public interface ISubLedger {
        ITrialBalance GetTrialBalance();
        Dictionary<int, IAccount> Accounts { get; }
    }
}