namespace Cellenza.MyFirst.Api.Infrastructures
{
    public interface ICache
    {
        void Insert<T>(string key, T clients);

        bool TryGet<T>(string key, out T o);
    }
}