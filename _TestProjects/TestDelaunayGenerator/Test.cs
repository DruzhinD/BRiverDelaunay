using CommonLib;
using CommonLib.Geometry;
//using DelaunayGenerator;
using GeometryLib.Aalgorithms;
using GeometryLib.Vector;
using MeshLib;
using RenderLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestDelaunayGenerator
{
    public class Test
    {
        IHPoint[] points = null;
        IHPoint[] Boundary = null;
        BoundarySet<BoundaryCreator> boundarySet = null;
        public Test() { }
        public void CreateRestArea(int idx)
        {
            const int N = 100;
            double h = 1.0 / (N - 1);
            //в него нужно помещать ВЕРШИНЫ границы области
            IHPoint[] boundary;
            BoundaryCreator boundaryCreator;
            switch (idx)
            {
                //Прямоугольник простой
                case 0:
                    Boundary = null;
                    boundary = new IHPoint[4]
                    {
                        new HPoint(0.3, 0.3),
                        new HPoint(0.3, 0.7),
                        new HPoint(0.7, 0.7),
                        new HPoint(0.7, 0.3),
                    };
                    points = new IHPoint[]
                    {
                        //new HPoint(0, 0),
                        //new HPoint(1, 0),
                        //new HPoint(1, 1),
                        //new HPoint(0, 1),
                        //new HPoint(0.5, 0.5),
                        //
                        new HPoint(0.1, 0.1),
                        new HPoint(0.1, 0.2),
                        new HPoint(0.1, 0.3),
                        new HPoint(0.1, 0.4),
                        new HPoint(0.1, 0.5),
                        new HPoint(0.1, 0.6),
                        new HPoint(0.1, 0.7),
                        new HPoint(0.1, 0.8),
                        new HPoint(0.1, 0.9),

                        new HPoint(0.2, 0.9),
                        new HPoint(0.2, 0.7),
                        new HPoint(0.2, 0.6),
                        new HPoint(0.2, 0.5),
                        new HPoint(0.2, 0.8),
                        new HPoint(0.2, 0.4),
                        new HPoint(0.2, 0.3),
                        new HPoint(0.2, 0.2),
                        new HPoint(0.2, 0.1),

                        new HPoint(0.3, 0.9),
                        new HPoint(0.3, 0.7),
                        new HPoint(0.3, 0.6),
                        new HPoint(0.3, 0.5),
                        new HPoint(0.3, 0.8),
                        new HPoint(0.3, 0.4),
                        new HPoint(0.3, 0.3),
                        new HPoint(0.3, 0.2),
                        new HPoint(0.3, 0.1),

                        new HPoint(0.4, 0.9),
                        new HPoint(0.4, 0.7),
                        new HPoint(0.4, 0.6),
                        new HPoint(0.4, 0.5),
                        new HPoint(0.4, 0.8),
                        new HPoint(0.4, 0.4),
                        new HPoint(0.4, 0.3),
                        new HPoint(0.4, 0.2),
                        new HPoint(0.4, 0.1),

                        new HPoint(0.5, 0.9),
                        new HPoint(0.5, 0.7),
                        new HPoint(0.5, 0.6),
                        new HPoint(0.5, 0.5),
                        new HPoint(0.5, 0.8),
                        new HPoint(0.5, 0.4),
                        new HPoint(0.5, 0.3),
                        new HPoint(0.5, 0.2),
                        new HPoint(0.5, 0.1),


                    };
                    Boundary = new BoundaryCreator(boundary, ref points).GetBoundaryPoints();
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
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.1, 0.1),
                        new HPoint(0.1, 0.9),
                        new HPoint(0.9, 0.9),
                        new HPoint(0.9, 0.1),

                    };
                    boundarySet = new BoundarySet<BoundaryCreator>(points);
                    boundarySet.Add(boundary);
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.3, 0.3),
                        new HPoint(0.3, 0.7),
                        new HPoint(0.7, 0.7),
                        new HPoint(0.7, 0.3),
                    };
                    boundarySet.Add(boundary);

                    //Boundary = new BoundaryCreator(boundary, ref points).GetBoundaryPoints();
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
                    Boundary = null;
                    boundary = new IHPoint[]
                    {
                        new HPoint(0.1, 0.1),
                        new HPoint(0.4, 0.4),
                        new HPoint(0.8, 0.4),
                        new HPoint(0.9, 0.1),
                        new HPoint(0.7, 0.2),
                     };
                    boundaryCreator = new BoundaryCreator(boundary, ref points);
                    Boundary = boundaryCreator.GetBoundaryPoints();
                    break;
                //Круглое множество
                case 3:
                    {
                        Boundary = null;
                        var width = 100;
                        var height = 100;
                        List<Vector2> samples = CircleDelaunayGenerator.SampleCircle(new Vector2(width / 2, height / 3), 220, 3);
                        points = new IHPoint[samples.Count];
                        for (int i = 0; i < samples.Count; i++)
                            points[i] = new HPoint(samples[i].X, samples[i].Y);
                    }
                    break;
                //Круглое множество с границей
                case 4:
                    {
                        Boundary = null;
                        var width = 100;
                        var height = 100;
                        List<Vector2> samples = CircleDelaunayGenerator.SampleCircle(new Vector2(width / 2, height / 3), 220, 3);
                        points = new IHPoint[samples.Count];
                        for (int i = 0; i < samples.Count; i++)
                            points[i] = new HPoint(samples[i].X, samples[i].Y);
                        boundary = new IHPoint[3]
                        {
                            new HPoint(0,0),
                            new HPoint(0,width),
                            new HPoint(height,width)
                        };
                        Boundary = new BoundaryCreator(boundary, ref points).GetBoundaryPoints();
                    }
                    break;
                //Круглое множество с вогнутой границей
                case 5:
                    {
                        var width = 100;
                        var height = 100;
                        boundary = new IHPoint[4]
                        {
                            new HPoint(0,0),
                            new HPoint(0,height),
                            new HPoint(width,height),
                            new HPoint(width,0)
                        };
                        List<Vector2> samples = CircleDelaunayGenerator.SampleCircle(new Vector2(width / 2, height / 3), 220, 3);
                        points = new IHPoint[samples.Count];
                        for (int i = 0; i < samples.Count; i++)
                            points[i] = new HPoint(samples[i].X, samples[i].Y);
                        Boundary = new BoundaryCreator(boundary, ref points).GetBoundaryPoints();
                    }
                    break;
            }
        }
        public void Run()
        {
            DelaunayMeshGenerator delaunator = new DelaunayMeshGenerator();

            delaunator.Generator(points, boundarySet);
            IMesh mesh = delaunator.CreateMesh();
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
