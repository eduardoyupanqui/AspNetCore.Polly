using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Metodo para hacer llamadas HttpClient, para su mejor uso se recomienda registrar el Client en la forma de HttpClientFactory
        ///     services.AddHttpClient<ICatalogService, CatalogService>(client =>
        ///     {
        ///         client.BaseAddress = new Uri(Configuration["BaseUrl"]);
        ///     });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">HttpClient asociado a la llamada, que ya es inyectada en la clase a llamar</param>
        /// <param name="verb">HttpMethod ya sea GET, POST, PUT , DELETE O PATCH</param>
        /// <param name="urlRelative">Url, es obligatoria pasar una ruta relativa, por Ejemplo: "solicitud/{idSolicitud}/procesos"</param>
        /// <param name="bodyParams">Objeto que ira en el Body del request, es Opcional</param>
        /// <param name="formData">Objeto que ira en el FormUrlEncodedContent del request, es Opcional</param>
        /// <param name="defaultMediaType">El media type que aceptara/enviara el request, por defecto es "application/json"</param>
        /// <param name="authorizationToken">Token que autorizacion si es necesario, se recomienda manejar un Hadler si es para todas las llamadas del endpoint</param>
        /// <param name="authorizationMethod">Tipo de Token a enviar, por defecto es "Bearer "</param>
        /// <param name="timeout">En desuso</param>
        /// <param name="headers">Headers adicionales que se incluiran en el request</param>
        /// <param name="cancellationToken">CancelattionToken si se maneja un Token de cancelacion de nivel superior</param>
        /// <returns></returns>
        public static async Task<T> InvokeWebApiAsync<T>(this HttpClient client, HttpMethod verb, string urlRelative, object bodyParams = null, IEnumerable<KeyValuePair<string, string>> formData = null, string defaultMediaType = "application/json", string authorizationToken = null, string authorizationMethod = "Bearer", int timeout = 0, IEnumerable<KeyValuePair<string, string>> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var request = new HttpRequestMessage(verb, new Uri(client.BaseAddress, new Uri(urlRelative, UriKind.RelativeOrAbsolute)));
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

            if (bodyParams != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, defaultMediaType);
            }

            if (formData != null && formData.Any())
            {
                request.Content = new FormUrlEncodedContent(formData);
            }

            using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCodeCustom();
                //var serializeSettings = new JsonSerializerSettings();
                //serializeSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyyMMddTHHmmssZ" });
                //return await response.Content.ReadAsAsync<T>(new[] {
                //        new JsonMediaTypeFormatter() {
                //            SerializerSettings = serializeSettings
                //        }
                //    });
                return await response.Content.ReadAsAsync<T>();
            }
        }
    }
}
