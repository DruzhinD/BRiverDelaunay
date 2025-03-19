using System;
using System.IO;
using System.Linq;
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
                    bool showForm = false;
                    bool usePointsFilter = true;
                    AreaBase area = null;
                    int countTests = 1;
                    int countPoints = 10_000;
                    GeneratorBase generator = new GeneratorFixed(200);
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
                            area = new GridArea(countPoints);
                            double small = 0.011115987;
                            int boundaryVertexesCnt = 10;
                            int BoundaryPointsCnt = (int)Math.Ceiling(0.07 * countPoints / boundaryVertexesCnt);
                            area.BoundaryGenerator = new GeneratorFixed(BoundaryPointsCnt);
                            var center = new HPoint(0.5 + small, 0.5 + small);
                            boundary = TruePolygonVertices(0.25, boundaryVertexesCnt, center);

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
                            SpecialTests(
                                startPointsCnt: 250_000, incrementPoints: 250_000, limitPoints: 250_000,
                                startBoundVertexes: 10, incrementBoundaryVertexes: 5, limitBoundVertexes: 200,
                                showForm: showForm);
                            break;
                        case ConsoleKey.Escape:
                            return;
                        default:
                            break;
                    }
                    if (area != null)
                    {
                        area.Initialize();
                        test.Run(area, usePointsFilter, countTests, showForm);
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

        static void SpecialTests(
            int startPointsCnt = 10_000, int incrementPoints = 5_000, int limitPoints = 300_000,
            int startBoundVertexes = 4, int incrementBoundaryVertexes = 4, int limitBoundVertexes = 20,
            bool showForm = false)
        {
            Test test = new Test(jsonLogger);
            AreaBase area = null;

            double small = 0.00001598798431234;
            double maxValue = 1;

            var center = new HPoint(0.5 + small, 0.5 + small);
            //цикл вариантов алгоритма: с использование предварительной фильтрации и без неё
            for (int i = 0; i < 2; i++)
            {
                bool usePointsFilter = i % 2 == 0;
                int currentBoundVertexes = startBoundVertexes;
                //цикл ограниченных областей
                while (currentBoundVertexes <= limitBoundVertexes)
                {
                    var boundary = TruePolygonVertices(maxValue/4, currentBoundVertexes, center);

                    int currentPointsCnt = startPointsCnt;
                    //цикл генерации исходных точек
                    while (currentPointsCnt <= limitPoints)
                    {
                        int currentBoundaryCnt = (int)Math.Ceiling(0.1 * currentPointsCnt / currentBoundVertexes);
                        area = new GridArea(currentPointsCnt, maxValue);
                        area.Initialize();
                        area.BoundaryGenerator = new GeneratorFixed(currentBoundaryCnt);
                        area.AddBoundary(boundary);
                        test.Run(area, usePointsFilter, 1, showForm);
                        currentPointsCnt += incrementPoints;
                    }
                    currentBoundVertexes += incrementBoundaryVertexes;
                }
            }
        }

        static IHPoint[] TruePolygonVertices(double radius, int vertexesCnt, IHPoint center)
        {
            var vertexes = new IHPoint[vertexesCnt];

            for (int i = 0; i < vertexesCnt; i++)
            {
                double theta = 2 * Math.PI * i / vertexesCnt;
                double x = center.X + radius * Math.Cos(theta);
                double y = center.Y + radius * Math.Sin(theta);
                vertexes[i] = new HPoint(x, y);
            }
            return vertexes;
        }
    }
}
