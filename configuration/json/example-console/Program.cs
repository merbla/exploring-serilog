using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using Microsoft.Extensions.Configuration;

namespace example_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
 
            Log.Verbose("This is a verbose statement");
            Log.Debug("This is a debug statement");
            Log.Information("This is a info statement");
            Log.Warning("This is a warning statement");
            Log.Error(new IndexOutOfRangeException(), "This is an error statement");
            Log.Fatal(new AggregateException(), "This is an fatal statement");
        }
    }
}
