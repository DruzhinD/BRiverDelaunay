using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// формирует множество граничных точек области построения
    /// </summary>
    public class BoundaryCreator
    {
        /// <summary>
        /// вершины, образующие границу
        /// </summary>
        IHPoint[] boundaryVertex;

        /// <summary>
        /// точки, принадлежащие границе, включая вершины границы
        /// </summary>
        IHPoint[] boundaryPoints;

        /// <summary>
        /// среднее расстояние между точками
        /// </summary>
        double avgDistance;

        public BoundaryCreator(IHPoint[] boundary, ref IHPoint[] basePoints)
        {
            this.boundaryVertex = boundary;
            Array.Sort(basePoints, new Comparator()); //сортируем точки

            //рассчет среднего расстояния между двумя точками из первых 10к точек
            //TODO: переделать
            //int amountDistance = 9999;
            //количество расстояний
            int amountDistance = basePoints.Length - 1;
            if (basePoints.Length < amountDistance + 1)
                amountDistance = basePoints.Length - 1;
            for (int i = 0; i < amountDistance; i++)
                avgDistance += this.GetDistance(basePoints[i], basePoints[i + 1]) / amountDistance;
        }

        double GetDistance(IHPoint p1, IHPoint p2)
        {
            return Math.Sqrt( Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) );
        }

        /// <summary>
        /// генерация граничных точек
        /// </summary>
        /// <returns>Массив граничных точек, включая вершины границы</returns>
        public IHPoint[] GetBoundaryPoints()
        {
            //если точки на границе уже сформированы, то возвращаем их
            if (boundaryPoints != null)
                return boundaryPoints;

            List<IHPoint> points = new List<IHPoint>();
            //проход по вершинам границы по часовой стрелке
            for (int i = 0; i < boundaryVertex.Length - 1; i++)
            {
                points.Add(boundaryVertex[i]);
                this.AddPointsBetweenVertexes(ref points, boundaryVertex[i], boundaryVertex[i + 1]);
                points.Add(boundaryVertex[i+1]);
            }

            //отдельно рассматриваем последнюю и первую вершины границы, которые образуют последнее ребро границы
            AddPointsBetweenVertexes(ref points, boundaryVertex[boundaryVertex.Length - 1], boundaryVertex[0]);

            boundaryPoints = points.ToArray();
            return this.boundaryPoints;
        }
        
        /// <summary>
        /// добавить новые точки между 2 вершинами
        /// </summary>
        void AddPointsBetweenVertexes(ref List<IHPoint> points, IHPoint v1, IHPoint v2)
        {
            int amountNewPoints = (int)Math.Floor(GetDistance(v1, v2) / avgDistance);
            //int amountNewPoints = 100;
            //интервалы смещения по координатно вдоль ребра границы
            double intervalX = (v2.X - v1.X) / amountNewPoints;
            double intervalY = (v2.Y - v1.Y) / amountNewPoints;

            //добавляем новые точки на ребра границы
            for (int j = 1; j <= amountNewPoints; j++)
                points.Add(new HPoint(v1.X + intervalX * j, v1.Y + intervalY * j));
        }

    }



    /// <summary>
    /// Компаратор для сортировки точек. <br/>
    /// Требуется для избегания реализации интерфейса IComparable в IHPoint, <br/>
    /// Иначе придется дописывать реализацию в производных классах
    /// </summary>
    public class Comparator : IComparer<IHPoint>
    {
        public int Compare(IHPoint @this, IHPoint other)
        {
            if (1e10 * (@this.X + 1) > @this.Y && 1e10 * (other.X + 1) > other.Y)
            {
                double a = 1e10 * @this.X + @this.Y;
                double b = 1e10 * other.X + other.Y;
                return a.CompareTo(b);
            }
            return @this.X.CompareTo(other.X);
        }
    }
}
