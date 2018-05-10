using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace httpclientfactory
{
    class Program
    {
        static void Main(string[] args) => Go().GetAwaiter().GetResult();

       static async Task Go()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddSerilog();
                });

            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            services.AddTransient<SerilogHandler>();
            services.AddHttpClient<GoogleClient>()
                .AddHttpMessageHandler<SerilogHandler>();

            var serviceProvider = services.BuildServiceProvider();

            // This is the Microsoft Logging interface
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("We are using Serilog!");
            var google = serviceProvider.GetRequiredService<GoogleClient>();
            await google.Get();
        }
    }

    class GoogleClient
    {
        HttpClient HttpClient { get; }
        public GoogleClient(HttpClient client)
        {
            HttpClient = client;
            HttpClient.BaseAddress = new Uri("https://google.com.au");

        }
        public async Task<HttpResponseMessage> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
    } 
    public class SerilogHandler : DelegatingHandler
    {
        private Serilog.ILogger _logger;

        public SerilogHandler(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            _logger.Debug("Request completed: {route} {method} {code} {headers}", request.RequestUri.Host, request.Method, response.StatusCode, request.Headers);
            return response;
        }
    }

     public class SerilogHttpMessageHandler2 : DelegatingHandler
    {
        private Microsoft.Extensions.Logging.ILogger _logger;

        public SerilogHttpMessageHandler2(Microsoft.Extensions.Logging.ILogger<SerilogHttpMessageHandler2> logger)
        {
            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // var stopwatch = Stopwatch.StartNew();

            // _logger.LogInformation("Sending HTTP request {HttpMethod} {Uri}", request.Method, request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);
            // _logger.LogInformation("Received HTTP response after {ElapsedMilliseconds}ms - {StatusCode}", stopwatch.GetElapsedTime().TotalMilliseconds, response.StatusCode);

            return response;
        }
    }
}
