using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

namespace example_console
{
    static class MyThemes
    {
        public static BeepingTheme Beep {get; } = new BeepingTheme();
    }

    class BeepingTheme : Serilog.Sinks.SystemConsole.Themes.ConsoleTheme
    {
        /// <summary>
        /// True if styling applied by the theme is written into the output, and can thus be
        /// buffered and measured.
        /// </summary>
        public override bool CanBuffer => false;

        /// <summary>
        /// Begin a span of text in the specified <paramref name="style"/>.
        /// </summary>
        /// <param name="output">Output destination.</param>
        /// <param name="style">Style to apply.</param>
        /// <returns></returns>
        protected override int ResetCharCount => 0;

        /// <summary>
        /// Reset the output to un-styled colors.
        /// </summary>
        /// <param name="output">The output.</param>
        public override void Reset(TextWriter output)
        {
            Console.ResetColor();
        }

        /// <summary>
        /// The number of characters written by the <see cref="Reset(TextWriter)"/> method.
        /// </summary>
        public override int Set(TextWriter output, ConsoleThemeStyle style)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            
            if(style == ConsoleThemeStyle.LevelFatal || style == ConsoleThemeStyle.LevelError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Beep();        
            }   
            else
            {
                Console.ForegroundColor = ConsoleColor.White;  
            }
            
            return 0;
        }
    }


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
