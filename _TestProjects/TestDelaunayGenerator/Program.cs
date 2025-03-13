using System;
using System.IO;
using CommonLib.Geometry;
using Serilog;
using Serilog.Formatting.Json;
using TestDelaunayGenerator.Areas;
using TestDelaunayGenerator.Boundary;

namespace TestDelaunayGenerator
{
    internal class Program
    {
        static ILogger jsonLogger;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //настройка логгера
            LoggerConfig();
            Log.Information("Запуск проекта .NET Framework 4.8.");

            jsonLogger = JsonLoggerConfig();

            ConsoleInterface();
        }


        static void ConsoleInterface()
        {
            Test test = new Test(specialLogger: jsonLogger);
            while (true)
            {
                Console.WriteLine("0: Простой квадрат (с границей)");
                Console.WriteLine("1: Равномерное распределение");
                Console.WriteLine("2: Нормальное (Гаусово) распределение");
                Console.WriteLine("3: Квадратная сетка");
                Console.WriteLine("4: Равномерное распределение (с границей)");
                Console.WriteLine("5: Нормальное (Гаусово) распределение (с границей)");
                Console.WriteLine("6: Квадратная сетка (с границей)");
                Console.WriteLine("7: звезда (сетка) (с границей)");
                Console.WriteLine("T: запуск тестов с записью логов в файлы логов");
                Console.WriteLine("Esc: выход");
                try
                {
                    IHPoint[] boundary = null;
                    bool showForm = true;
                    bool usePointsFilter = false;
                    AreaBase area = null;
                    int count = 1;
                    GeneratorBase generator = new GeneratorFixed(600);
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                    switch (consoleKeyInfo.Key)
                    {
                        case ConsoleKey.D0:
                            area = new SimpleSquareArea();
                            break;
                        case ConsoleKey.D1:
                            area = new UniformArea();
                            break;
                        case ConsoleKey.D2:
                            area = new GaussArea(1_000_000, mean: 0.5, stdDev: 0.3);
                            break;
                        case ConsoleKey.D3:
                            area = new GridArea();
                            break;
                        //с границей
                        case ConsoleKey.D4:
                            boundary = new IHPoint[]
                            {
                                new HPoint(0.1,0.1),
                                new HPoint(0.2,0.4),
                                new HPoint(0.1,0.8),
                                new HPoint(0.4,0.5),
                                new HPoint(0.7,0.8),
                                new HPoint(0.7,0.1),
                            };
                            area = new UniformArea(valueMin: 0, valueMax: 1);
                            area.BoundaryGenerator = generator;
                            area.AddBoundary(boundary);
                            break;
                        case ConsoleKey.D5:
                            area = new GaussArea(mean: 0.5, stdDev: 0.1);
                            area.BoundaryGenerator = generator;
                            boundary = new IHPoint[]
                            {
                                new HPoint(0.1,0.1),
                                new HPoint(0.1,0.8),
                                new HPoint(0.4,0.5),
                                new HPoint(0.7,0.8),
                                new HPoint(0.7,0.1),
                            };
                            area.AddBoundary(boundary);
                            break;
                        case ConsoleKey.D6:
                            area = new GridArea(10_000);
                            double small = 0.011115987;
                            area.BoundaryGenerator = new GeneratorFixed(120);
                            boundary = new IHPoint[]
                            {
                                new HPoint(0.1+small,0.1+small),
                                new HPoint(0.1+small,0.8+small),
                                new HPoint(0.8+small,0.8+small),
                                new HPoint(0.8+small,0.1+small),
                            };
                            area.AddBoundary(boundary);
                            boundary = new IHPoint[]
                            {
                                new HPoint(0.3+small,0.3+small),
                                new HPoint(0.3+small,0.6+small),
                                new HPoint(0.6+small,0.6+small),
                                new HPoint(0.6+small,0.3+small),
                            };
                            area.AddBoundary(boundary);

                            break;
                        case ConsoleKey.D7:
                            area = new GridArea();
                            area.BoundaryGenerator = generator;
                            boundary = new IHPoint[]
                            {
                                new HPoint(0.5001,0.9001),
                                new HPoint(0.6001,0.4001),
                                new HPoint(0.9001,0.38001),
                                new HPoint(0.6001,0.2001),
                                new HPoint(0.9001,0.0101),
                                new HPoint(0.5001,0.1001),
                                new HPoint(0.1001,0.0101),
                                new HPoint(0.29, 0.135), //обычный узел заскочил на сетку
                                new HPoint(0.4011,0.2001),
                                new HPoint(0.0101,0.38001),
                                new HPoint(0.4001,0.4001),
                            };
                            area.AddBoundary(boundary);
                            break;
                        case ConsoleKey.T:
                            SpecialTests(usePointsFilter);
                            break;
                        case ConsoleKey.Escape:
                            return;
                        default:
                            break;
                    }
                    if (area != null)
                    {
                        area.Initialize();
                        test.Run(area, usePointsFilter, count, showForm);
                    }

                    Console.Clear();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
        }

        #region Конфигурация логгеров
        /// <summary>
        /// Настройка логгера для текущего проекта, используется <see cref="Log.Logger"/>
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


        /// <summary>
        /// Настройка логгера в формате json
        /// </summary>
        /// <returns></returns>
        static ILogger JsonLoggerConfig()
        {
            string logPath = Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, Properties.Resources.specialLogPath
                );
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.WriteTo.File(formatter: new CsvFormatter(), path: logPath)
                .WriteTo.File(formatter: new JsonFormatter(closingDelimiter: ",\r\n"), path: logPath)
                .CreateLogger();
            return logger;
        }
        #endregion

        static void SpecialTests(bool usePointsFilter = true, int increment = 5_000, int start = 10_000,  int limit = 300_000)
        {
            Test test = new Test(jsonLogger);
            AreaBase area = null;

            double small = 0.01111598798431234;
            var boundary = new IHPoint[]
            {
                new HPoint(0.1+small,0.1+small),
                new HPoint(0.1+small,0.8+small),
                new HPoint(0.8+small,0.8+small),
                new HPoint(0.8+small,0.1+small),
            };

            var boundary2 = new IHPoint[]
            {
                new HPoint(0.3+small,0.3+small),
                new HPoint(0.3+small,0.6+small),
                new HPoint(0.6+small,0.6+small),
                new HPoint(0.6+small,0.3+small),
            };
            var boundary3 = new IHPoint[]
            {
                new HPoint(0.4+small,0.4+small),
                new HPoint(0.4+small,0.5+small),
                new HPoint(0.5+small,0.5+small),
                new HPoint(0.5+small,0.4+small),
            };

            int currentCnt = start;
            int currentBoundaryCnt = 2000;
            //int boundaryPointsIncrement = (2500 - currentBoundaryCnt) / (limit / increment);
            while (currentCnt <= limit)
            {
                area = new GridArea(currentCnt);
                area.BoundaryGenerator = new GeneratorFixed(currentBoundaryCnt);
                area.AddBoundary(boundary);
                area.AddBoundary(boundary2);
                area.AddBoundary(boundary3);
                area.Initialize();
                test.Run(area, usePointsFilter, 1, false);
                currentCnt += increment;
                //currentBoundaryCnt += boundaryPointsIncrement;
            }

        }
    }
}
