using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Polly;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var cbrConfig = new CBR
            {
                HandlerLifetime = 5,
                ExceptionsAllowedBeforeBreaking = 2,
                DurationOfBreak = 1,
                RetryCount = 3,
                SleepDuration = 2,
                UserAgent = "YUP"
            };
            //CreandoServiceProvider [Test]
            IServiceCollection service = new ServiceCollection();
            service
                .AddHttpClient<IDemoWebApiClient, DemoWebApiClient>()
                .ConfigureHttpClient(c =>
                {
                        c.BaseAddress = new Uri("http://localhost:5000/");
                        //c.DefaultRequestHeaders.Add("Accept", "application/json");
                        c.DefaultRequestHeaders.Accept.Clear();
                    c.MaxResponseContentBufferSize = int.MaxValue;

                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(cbrConfig.HandlerLifetime))
                .AddPolicyHandler(Policies.GetRetryPolicy(cbrConfig))
                //.AddPolicyHandler(request => request.Method == HttpMethod.Get ? Policies.GetRetryPolicy(cbrConfig) : Policies.GetNoOpPolicy())
                .AddPolicyHandler(Policies.GetCircuitBreakerPolicy(cbrConfig))
                //Abreviado
                //.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(cbrConfig.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(cbrConfig.SleepDuration, retryAttempt))))
                //.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(cbrConfig.ExceptionsAllowedBeforeBreaking, TimeSpan.FromSeconds(cbrConfig.DurationOfBreak)))
                ;
            var serviceProvider = service.BuildServiceProvider();

            var webApiClient = serviceProvider.GetService<IDemoWebApiClient>();


            var resultado = webApiClient.GetConfiguracion();


            var xx = JsonConvert.SerializeObject(resultado);
            Console.WriteLine(xx);
            Console.ReadLine();
        }
    }



    public class CBR {
        public int HandlerLifetime { get; set; }
        public int ExceptionsAllowedBeforeBreaking { get; set; }
        public int DurationOfBreak { get; set; }
        public int RetryCount { get; set; }
        public double SleepDuration { get; set; }
        public string UserAgent { get; set; }
    }
}
