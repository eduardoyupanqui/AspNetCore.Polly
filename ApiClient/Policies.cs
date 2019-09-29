using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApiClient
{
    public class Policies
    {
        internal static IAsyncPolicy<HttpResponseMessage> GetNoOpPolicy()
        {
            return Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
        }
        internal static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(CBR cbr)
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    .WaitAndRetryAsync(cbr.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(cbr.SleepDuration, retryAttempt)));
        }

        internal static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(CBR cbr)
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(cbr.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMinutes(cbr.DurationOfBreak))
                    //.CircuitBreakerAsync<HttpResponseMessage>(cbr.ExceptionsAllowedBeforeBreaking,
                    //                                          TimeSpan.FromMinutes(cbr.DurationOfBreak),
                    //                                          onBreak: onBreak, onReset: onReset)
                    ;
        }

        static Action<DelegateResult<HttpResponseMessage>, TimeSpan, Context> onBreak = (exception, timespan, context) => {  
            
        };
        static Action<Context> onReset = context => {  

        };
    }
}
