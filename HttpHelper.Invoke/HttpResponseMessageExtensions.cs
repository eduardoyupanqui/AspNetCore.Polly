using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpHelper.Invoke
{
    public static class HttpResponseMessageExtensions
    {
        //https://github.com/microsoft/referencesource/blob/master/System/net/System/Net/Http/HttpResponseMessage.cs#L142
        public static HttpResponseMessage EnsureSuccessStatusCodeCustom(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string content = response.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
                // Disposing the content should help users: If users call EnsureSuccessStatusCode(), an exception is
                // thrown if the response status code is != 2xx. I.e. the behavior is similar to a failed request (e.g.
                // connection failure). Users don't expect to dispose the content in this case: If an exception is 
                // thrown, the object is responsible fore cleaning up its state.
                if (response.Content != null)
                    response.Content.Dispose();
                throw new HttpResponseException(response.StatusCode, response.ReasonPhrase, content);
            }
            return response;
        }
    }
}
