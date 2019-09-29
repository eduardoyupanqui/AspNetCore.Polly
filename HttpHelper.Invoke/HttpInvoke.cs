using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpHelper.Invoke
{
    public static class HttpInvoke
    {
        public static async Task<T> InvokeWebApiAsync<T>(this HttpClient client, HttpMethod verb, string url, string jsonData = null, string defaultMediaType = "application/json", string authorizationToken = null, string authorizationMethod = "Bearer", int timeout = 0, IEnumerable<KeyValuePair<string, string>> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var request = new HttpRequestMessage(verb, new Uri(client.BaseAddress, new Uri(url, UriKind.RelativeOrAbsolute)));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(defaultMediaType));

            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (!string.IsNullOrWhiteSpace(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, defaultMediaType);
            }

            using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
            {
                await response.EnsureSuccessStatusCodeAsync();
                return await response.Content.ReadAsAsync<T>();
            }
        }
    }
}
