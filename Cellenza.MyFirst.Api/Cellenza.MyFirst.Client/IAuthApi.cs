using System.Threading.Tasks;

namespace Cellenza.MyFirst.Client
{
    public interface IAuthApi
    {
        Task<Token> Connect(string username, string password);
    }
}