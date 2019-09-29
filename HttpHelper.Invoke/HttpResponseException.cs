using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HttpHelper.Invoke
{
    public class HttpResponseException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ReasonPhrase { get; private set; }
        public string Content { get; private set; }

        public HttpResponseException(HttpStatusCode statusCode, string reasonPhrase, string content) : 
            base(MakeMessage(statusCode, reasonPhrase))
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Content = content;
        }

        private static string MakeMessage(HttpStatusCode statusCode, string reasonPhrase)
        {
            return string.Format(
                    "El código de estado de respuesta no indica éxito: {0} ({1}).",
                    (int)statusCode,
                    reasonPhrase);
        }
    }
}
