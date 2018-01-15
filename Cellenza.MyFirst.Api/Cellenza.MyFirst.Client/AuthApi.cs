using System.Threading.Tasks;

namespace Cellenza.MyFirst.Client
{
    public class AuthApi : MyFirstBaseApi, IAuthApi
    {
        public AuthApi(ClientConfig config) : base(config)
        {
        }

        public Task<Token> Connect(string username, string password)
        {
            return this.PostStringAsync<Token>("/connect/token",
                $"username={username}&password={password}&grant_type=Password&client_id=robot");
        }
    }
}