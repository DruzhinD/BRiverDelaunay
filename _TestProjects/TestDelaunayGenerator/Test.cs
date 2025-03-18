using CommonLib;
using CommonLib.Geometry;
using MeshLib;
using RenderLib;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Serilog;
using TestDelaunayGenerator.Areas;

namespace TestDelaunayGenerator
{
    public class Test
    {
        public Test(ILogger specialLogger = null)
        {
            this.specialLogger = specialLogger;
        }

        protected ILogger specialLogger = null;

        public void Run(AreaBase area, bool usePointsFilter = true, int count = 1, bool openForm = true)
        {
            for (int i = 0; i < count; i++)
            {
                IHPoint[] points = area.Points;
                BoundaryContainer boundaryContainer = area.BoundaryContainer;
                //TODO переместить в AreaBase
                //если граница задана, то расширяем исходное множество узлов множеством граничных узлов
                if (boundaryContainer != null)
                {
                    IHPoint[] boundary = boundaryContainer.AllBoundaryKnots;
                    int exPointsLength = points.Length;
                    Array.Resize(ref points, points.Length + boundary.Length);
                    boundary.CopyTo(points, exPointsLength);
                }

                DelaunayMeshGenerator delaunator = new DelaunayMeshGenerator(points, boundaryContainer, usePointsFilter);
                //измерение времени предварительной фильтрации
                Stopwatch watch = Stopwatch.StartNew();
                delaunator.PreFilterPoints();
                double filterPointsSeconds = watch.Elapsed.TotalSeconds;

                //измерение времени генерации сетки
                watch = Stopwatch.StartNew();
                delaunator.Generator();
                double genSeconds = watch.Elapsed.TotalSeconds;

                //фильтрация треугольников
                watch = Stopwatch.StartNew();
                IMesh mesh = delaunator.CreateMesh();
                double filterSeconds = watch.Elapsed.TotalSeconds;

                var log = new TriangulationLog(area, mesh, filterPointsSeconds, genSeconds, filterSeconds, usePointsFilter);
                Log.Information(log.ToString());
                if (specialLogger != null)
                    specialLogger.Information("{@info}", log);
                
                //отобразить форму
                if (openForm)
                    ShowMesh(mesh);
            }

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
