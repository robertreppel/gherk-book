namespace Bookkeeper.Infrastructure.Interfaces {
    public interface IBusiness {
        void Add<T>(object accountingArtifact) where T: ILedger;
        T Find<T>(string key) where T: ILedger;
        T Find<T>(int key) where T: IAccount;
    }
}