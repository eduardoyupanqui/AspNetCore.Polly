using System.Collections.Generic;

namespace ApiClient
{
    internal interface IDemoWebApiClient
    {
        IEnumerable<object> GetConfiguracion();
    }
}