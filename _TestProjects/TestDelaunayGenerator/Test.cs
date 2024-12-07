using CommonLib;
using CommonLib.Geometry;
//using DelaunayGenerator;
using GeometryLib.Aalgorithms;
using GeometryLib.Vector;
using MeshLib;
using RenderLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace TestDelaunayGenerator
{
    public class Test
    {
        IHPoint[] points = null;
        BoundarySet boundarySet = null;
        public Test() { }
        public void CreateRestArea(int idx)
        {
            Console.WriteLine();
            const int N = 100;
            double h = 1.0 / (N - 1);
            //в него нужно помещать ВЕРШИНЫ границы области
            IHPoint[] boundary;
            switch (idx)
            {
                //Прямоугольник простой
                case 0:
                    points = new IHPoint[]
                    {
                        new HPoint(0, 0),
                        new HPoint(0, 1),
                        new HPoint(1, 1),
                        new HPoint(1, 0),
                        new HPoint(0.5, 0.95),
                        new HPoint(0.5, 0.05),

                    };
                    //boundary = new IHPoint[4]
                    //{
                    //    new HPoint(0.31111, 0.31111),
                    //    new HPoint(0.31111, 0.71111),
                    //    new HPoint(0.71111, 0.71111),
                    //    new HPoint(0.71111, 0.31111),
                    //};
                    break;
                //Прямоугольник большой
                case 1:

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
                    var watch = Stopwatch.StartNew();

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

                    //boundarySet = new BoundarySet(points);
                    //boundary = new IHPoint[]
                    //{

                    //    new HPoint(0.0+0.01, 0.0+0.01),
                    //    new HPoint(0.0+0.01, 1.0-0.01),
                    //    new HPoint(1.0-0.01, 1.0-0.01),
                    //    new HPoint(1.0-0.01, 0.0+0.01),
                    //};
                    //boundarySet.Add(boundary);
                    ////boundary = new IHPoint[]
                    ////{
                    ////    new HPoint(0.15, 0.6),
                    ////    new HPoint(0.2, 0.1),
                    ////    new HPoint(0.1, 0.1),
                    ////};
                    ////boundarySet.Add(boundary);
                    //boundary = new IHPoint[]
                    //{
                    //    new HPoint(0.1, 0.1),
                    //    new HPoint(0.1, 0.3),
                    //    new HPoint(0.105, 0.15),
                    //    new HPoint(0.11, 0.3),
                    //    new HPoint(0.11, 0.1),
                    //};
                    //boundarySet.Add(boundary);
                    break;
                //Трапеция
                case 2:
                    points = new IHPoint[N * N];
                    for (int i = 0; i < N; i++)
                    {
                        double hx = h - (h / 3 * i) / N;
                        for (int j = 0; j < N; j++)
                            points[i * N + j] = new HPoint(h * i, hx * j);
                    }
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.1, 0.1),
                        new HPoint(0.4, 0.4),
                        new HPoint(0.8, 0.4),
                        new HPoint(0.9, 0.1),
                        new HPoint(0.7, 0.2),
                     };
                    boundarySet = new BoundarySet(points);
                    boundarySet.Add(boundary);
                    break;
                //Круглое множество с вогнутой границей
                case 3:
                    {
                        var width = 100;
                        var height = 100;
                        List<Vector2> samples = CircleDelaunayGenerator.SampleCircle(new Vector2(width / 2, height / 3), 220, 2.5f);
                        points = new IHPoint[samples.Count];
                        for (int i = 0; i < samples.Count; i++)
                            points[i] = new HPoint(samples[i].X, samples[i].Y);
                        //Boundary = new BoundaryCreator(boundary, ref points).GetBoundaryPoints();
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
                        List<HPoint> points = new List<HPoint>();
                        Random rnd = new Random();
                        for (int i = 0; i < 30 * 30; i++)
                        {
                            //points.Add(new HPoint(rnd.NextDouble(), rnd.NextDouble()));
                            points.Add(new HPoint(rnd.Next(1, 100), rnd.Next(1, 100)));
                            points.Add(new HPoint(rnd.Next(40, 60) * rnd.NextDouble(), rnd.Next(40, 60) * rnd.NextDouble()));
                        }
                        this.points = points.ToArray();
                        //boundarySet = new BoundarySet(this.points);
                        //var (width, height, offset) = (40, 40, 10);
                        //boundary = new IHPoint[]
                        //{
                        //    new HPoint(40, 10),
                        //    new HPoint(40, 40),
                        //    new HPoint(10, 40),
                        //    new HPoint(10, 10),
                        //};
                        //boundarySet.Add(boundary);
                        break;
                    }
            }
        }
        public void Run()
        {
            DelaunayMeshGenerator delaunator = new DelaunayMeshGenerator();

            var watch = Stopwatch.StartNew();
            delaunator.Generator(points, boundarySet);
            string msg = $"Рассчет триангуляции {this.points.Length} {watch.Elapsed.TotalSeconds} сек.";
            Console.WriteLine(msg);

            watch = Stopwatch.StartNew();
            IMesh mesh = delaunator.CreateMesh();
            msg = $"Генерация сетки (TriMesh) {this.points.Length} {watch.Elapsed.TotalSeconds} сек.";
            Console.WriteLine(msg);

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
