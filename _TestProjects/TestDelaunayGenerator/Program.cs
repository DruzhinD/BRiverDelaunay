using System;
using System.IO;
using Serilog;

namespace TestDelaunayGenerator
{
    internal class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //настройка логгера
            LoggerConfig();
            Log.Information("Запуск проекта .NET Framework 4.8.");
            while (true)
            {
                Test test = new Test();
                Console.WriteLine("Выор тестовой области:");
                Console.WriteLine("1. Прямоугольник простой");
                Console.WriteLine("2. Прямоугольник большой");
                Console.WriteLine("3. Трапеция");
                Console.WriteLine("4. Круглое множество с вогнутой границей");
                Console.WriteLine("5. Случайная генерация");
                Console.WriteLine("Esc: выход");
                try
                {
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                    if (consoleKeyInfo.Key == ConsoleKey.Escape)
                        return;
                    if (consoleKeyInfo.Key == ConsoleKey.D1)
                        test.Run(0);
                    if (consoleKeyInfo.Key == ConsoleKey.D2)
                        test.Run(1);
                    if (consoleKeyInfo.Key == ConsoleKey.D3)
                        test.Run(2);
                    if (consoleKeyInfo.Key == ConsoleKey.D4)
                        test.Run(3);
                    if (consoleKeyInfo.Key == ConsoleKey.D5)
                        test.Run(4);
                    if (consoleKeyInfo.Key == ConsoleKey.D6)
                        test.Run(5);
                    Console.Clear();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
        }

        /// <summary>
        /// Настройка логгера для текущего проекта, используется <see cref="Serilog.Log.Logger"/>
        /// </summary>
        static void LoggerConfig()
        {
            string logPath = Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, Properties.Resources.logPath
                );
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: Properties.Resources.logFormat)
                .WriteTo.File(path: logPath, outputTemplate: Properties.Resources.logFormat)
                .CreateLogger();
        }
    }
}
