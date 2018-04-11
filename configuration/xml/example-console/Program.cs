using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Core; 

namespace example_console
{
    class Program
    {
        static void Main(string[] args)
        { 
            // Get a path to the running folder
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var configFile = Path.GetFullPath(Path.Combine(basePath, "appsettings.config"));

            // Apply the config to the logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings(filePath: configFile)
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