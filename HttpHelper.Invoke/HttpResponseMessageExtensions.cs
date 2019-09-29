using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HttpHelper.Invoke
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpResponseMessage EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            //var content = await response.Content.ReadAsStringAsync();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (response.Content != null)
                response.Content.Dispose();
            
            
            throw new HttpResponseException(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}
