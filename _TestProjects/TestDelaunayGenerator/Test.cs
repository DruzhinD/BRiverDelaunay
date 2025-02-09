using CommonLib;
using CommonLib.Geometry;
using GeometryLib.Aalgorithms;
using GeometryLib.Vector;
using MeshLib;
using RenderLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Serilog;

namespace TestDelaunayGenerator
{
    public class Test
    {
        string areaType = "none"; //тип отрисовываемой области
        IHPoint[] points = null;
        BoundarySet boundarySet = null;
        public Test() { }
        public void CreateRestArea(int idx)
        {
            const int N = 100;
            double h = 1.0 / (N - 1);
            //в него нужно помещать ВЕРШИНЫ границы области
            IHPoint[] boundary;
            switch (idx)
            {
                //Прямоугольник простой
                case 0:
                    areaType = "Прямоугольник простой";
                    points = new IHPoint[]
                    {
                        new HPoint(0, 0),
                        new HPoint(0, 1),
                        new HPoint(1, 1),
                        new HPoint(1, 0),
                        new HPoint(0.5, 0.95),
                        new HPoint(0.5, 0.05),

                    };
                    break;
                //Прямоугольник большой
                case 1:
                    areaType = "Прямоугольник большой";
                    // массивы для псевдослучайного микро смещения координат узлов
                    double[] dxx = {0.0000001, 0.0000005, 0.0000002, 0.0000006, 0.0000002,
                            0.0000007, 0.0000003, 0.0000001, 0.0000004, 0.0000009,
                            0.0000000, 0.0000003, 0.0000006, 0.0000004, 0.0000008 };
                    double[] dyy = { 0.0000005, 0.0000002, 0.0000006, 0.0000002, 0.0000004,
                             0.0000007, 0.0000003, 0.0000001, 0.0000001, 0.0000004,
                             0.0000009, 0.0000000, 0.0000003, 0.0000006,  0.0000008 };
                    int idd = 0;
                    points = new IHPoint[N * N];
                    for (int i = 0; i < N; i++)
                        for (int j = 0; j < N; j++)
                        {
                            // тряска координат
                            points[i * N + j] = new HPoint(h * i + dxx[idd], h * j + dyy[idd]);
                            //  points[i * N + j] = new HPoint(h * i, h * j );
                            idd++;
                            idd = idd % dxx.Length;
                        }

                    boundarySet = new BoundarySet(points);

                    boundary = new IHPoint[]
                    {
                        new HPoint(0.1, 0.1),
                        new HPoint(0.1, 0.9),
                        new HPoint(0.9, 0.9),
                        new HPoint(0.9, 0.1),

                    };
                    boundarySet.Add(boundary);

                    boundary = new IHPoint[]
                    {
                        new HPoint(0.3, 0.3),
                        new HPoint(0.3, 0.7),
                        new HPoint(0.7, 0.7),
                        new HPoint(0.7, 0.3),
                    };
                    boundarySet.Add(boundary);

                    boundary = new IHPoint[]
                    {
                        new HPoint(0.0+0.01, 0.0+0.01),
                        new HPoint(0.0+0.01, 1.0-0.01),
                        new HPoint(1.0-0.01, 1.0-0.01),
                        new HPoint(1.0-0.01, 0.0+0.01),
                    };
                    boundarySet.Add(boundary);
                    break;
                //Трапеция
                case 2:
                    areaType = "Трапеция";
                    points = new IHPoint[N * N];
                    for (int i = 0; i < N; i++)
                    {
                        double hx = h - (h / 3 * i) / N;
                        for (int j = 0; j < N; j++)
                            points[i * N + j] = new HPoint(h * i, hx * j);
                    }
                    boundarySet = new BoundarySet(points);
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.1, 0.1),
                        new HPoint(0.4, 0.4),
                        new HPoint(0.8, 0.4),
                        new HPoint(0.9, 0.1),
                        new HPoint(0.7, 0.2),
                     };
                    boundarySet.Add(boundary);
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.4, 0.2),
                        new HPoint(0.4, 0.3),
                        new HPoint(0.5, 0.3),
                        new HPoint(0.5, 0.2),
                     };
                    boundarySet.Add(boundary);
                    break;
                //Круглое множество с вогнутой границей
                case 3:
                    areaType = "Круглое множество";
                    {
                        var width = 100;
                        var height = 100;
                        List<Vector2> samples = CircleDelaunayGenerator.SampleCircle(new Vector2(width / 2, height / 3), 220, 2.5f);
                        points = new IHPoint[samples.Count];
                        for (int i = 0; i < samples.Count; i++)
                            points[i] = new HPoint(samples[i].X, samples[i].Y);
                        boundarySet = new BoundarySet(points);
                        int offset = 40;
                        boundary = new IHPoint[4]
                        {
                            new HPoint(-width/1.5+offset,-height/1.5+offset),
                            new HPoint(-width/1.5+offset,height/1.5+offset),
                            new HPoint(width/1.5+offset,height/1.5+offset),
                            new HPoint(width/1.5+offset,-height/1.5+offset)
                        };
                        boundarySet.Add(boundary);
                        boundary = new IHPoint[]
                        {
                            new HPoint(-width*2+offset, offset),
                            new HPoint(+offset, height*2+offset),
                            new HPoint(width*2+offset, offset),
                            new HPoint(offset, -height*2+offset),
                        };
                        boundarySet.Add(boundary);
                    }
                    break;
                case 4:
                    {
                        areaType = "Случайная генерация";
                        Random rnd = new Random();
                        this.points = new IHPoint[100 * 100];
                        for (int i = 0; i < this.points.Length; i++)
                        {
                            var p = new HPoint(rnd.Next(0, 1000) * rnd.NextDouble(), rnd.Next(0, 1000) * rnd.NextDouble());
                            this.points[i] = p;
                        }
                        break;
                    }
            }
        }
        public void Run(int areaId)
        {
            CreateRestArea(areaId);
            Log.Information($"Выбрана область с {areaId} id | тип: {areaType}");
            DelaunayMeshGenerator delaunator = new DelaunayMeshGenerator();

            var watch = Stopwatch.StartNew();
            delaunator.Generator(points, boundarySet);
            Log.Information($"Рассчет триангуляции {this.points.Length}шт {watch.Elapsed.TotalSeconds} сек.");
            bool border = boundarySet != null;
            Log.Information($"Граница:{border} | Начальное количество точек:{points.Length}шт | Колво после генерации:{delaunator.Points.Length}шт");

            watch = Stopwatch.StartNew();
            IMesh mesh = delaunator.CreateMesh();
            Log.Information($"Генерация сетки (TriMesh) {delaunator.Points.Length}/{this.points.Length}шт {watch.Elapsed.TotalSeconds} сек.");

            IConvexHull ch = new ConvexHull();
            ShowMesh(mesh);
        }

        protected void ShowMesh(IMesh mesh)
        {
            if (mesh != null)
            {
                SavePoint data = new SavePoint();
                data.SetSavePoint(0, mesh);
                double[] xx = mesh.GetCoords(0);
                double[] yy = mesh.GetCoords(1);
                data.Add("Координата Х", xx);
                data.Add("Координата Y", yy);
                data.Add("Координаты ХY", xx, yy);
                Form form = new ViForm(data);
                form.ShowDialog();
            }
        }

    }
}
