using System.Collections.Generic;
using System.Net.Http;
using HttpHelper.Invoke;


using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace ApiClient
{
    internal class DemoWebApiClient : IDemoWebApiClient
    {
        private readonly HttpClient httpClient;
        public DemoWebApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;

        }

        public IEnumerable<object> GetConfiguracion() 
        {
            var xxx = httpClient.DefaultRequestHeaders;
            var resultado = httpClient.InvokeWebApiAsync<IEnumerable<object>>(HttpMethod.Get, "/api/Values").Result;
            return resultado;
        }

        public Task<IEnumerable<object>> GetConfiguracionAsync()
        {
            return httpClient.InvokeWebApiAsync<IEnumerable<object>>(HttpMethod.Get, "/api/Values");
        }

        public void Prueba() {
            var xxx = httpClient.GetAsync("api/Values");
        }

    }
}