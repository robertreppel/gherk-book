using Bookkeeper;
using Bookkeeper.Accounting;
using Bookkeeper.Infrastructure.Interfaces;
using NUnit.Framework;
using SharpTestsEx;

namespace TestBookkeeper
{
    [TestFixture]
    class TestBusiness
    {
        [Test]
        public void ShouldFindLedger() {
            var business = Business.Create();
            var ledger = Ledger.CreateLedger(123, "Accounts Receivable");
            business.Add<ILedger>(ledger);

            var ledgerFound = business.Find<ILedger>("Accounts Receivable");

            ledgerFound.Should().Not.Be.Null();
        }

        [Test]
        public void ShouldFindAccount()
        {
            var business = Business.Create();
            var ledger = Ledger.CreateLedger(123, "Accounts Receivable");
            ledger.AddAccount(2000, "John Forbes", AccountType.Revenue);

            business.Add<ILedger>(ledger);

            var account = business.Find<IAccount>(2000);

            account.Should().Not.Be.Null();
        }
    }
}
