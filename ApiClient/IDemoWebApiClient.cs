using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiClient
{
    internal interface IDemoWebApiClient
    {
        IEnumerable<object> GetConfiguracion();
        Task<IEnumerable<object>> GetConfiguracionAsync();
    }
}