using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace example_api
{
    class SimpleSerilogMiddleware
    {
        static readonly ILogger Log = Serilog.Log.ForContext<SimpleSerilogMiddleware>();

        readonly RequestDelegate _next;

        public SimpleSerilogMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
 
            await _next(httpContext);
            var statusCode = httpContext.Response?.StatusCode;
            Log.Information("{client} {protoco} {path} {code} {length} ", httpContext.Connection.RemoteIpAddress, httpContext.Request.Method, httpContext.Request.Protocol, httpContext.Request.Path, httpContext.Response?.StatusCode, httpContext.Response?.ContentLength);
        }
    }
}