using System.Collections.Generic;
using System.Threading.Tasks;
using Cellenza.MyFirst.Dto;

namespace Cellenza.MyFirst.Client
{
    public interface IClientApi
    {
        Task<List<ClientV2Dto>> GetAll();
    }
}