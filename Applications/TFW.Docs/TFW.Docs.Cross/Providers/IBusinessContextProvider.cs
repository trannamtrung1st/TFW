namespace TFW.Docs.Cross.Providers
{
    public interface IBusinessContextProvider
    {
        BusinessContext BusinessContext { get; }
    }

    public class NullBusinessContextProvider : IBusinessContextProvider
    {
        public BusinessContext BusinessContext => null;
    }
}
