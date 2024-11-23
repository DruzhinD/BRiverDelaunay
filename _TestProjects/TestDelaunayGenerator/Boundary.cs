using CommonLib.Geometry;
using System;
using System.Collections.Generic;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// формирует множество граничных точек области построения
    /// </summary>
    public class Boundary
    {
        public IHPoint this[int index]
        {
            get => boundaryPoints[index];
        }

        public int Length => boundaryPoints.Length;
        /// <summary>
        /// вершины, образующие границу
        /// </summary>
        IHPoint[] boundaryVertex;

        IHPoint[] boundaryPoints;
        /// <summary>
        /// точки, принадлежащие границе, включая вершины границы
        /// </summary>
        public IHPoint[] BoundaryPoints { get
            {
                if (boundaryPoints == null)
                    throw new ArgumentNullException($"{nameof(boundaryPoints)} не задана");
                return boundaryPoints;
            } }

        /// <summary>
        /// флаг для генерации точек. True - количество точек на границе будет иметь фикс размер; <br/>
        /// False - количество точек будет зависеть от точек триангуляции
        /// </summary>
        bool fixedAmount = true;

        protected Boundary(IHPoint[] boundary, bool fixedAmount = true)
        {
            this.boundaryVertex = boundary;
            this.fixedAmount = fixedAmount;
        }


        public static Boundary Generate(IHPoint[] basePoints, IHPoint[] boundaryVertexes, bool fixedAmount = true)
        {
            Boundary boundary = new Boundary(boundaryVertexes, fixedAmount);
            boundary.InitializeBoundary(basePoints);
            return boundary;
        }


        ///<summary>расстояние между точками (длина отрезка)</summary>
        double GetDistance(IHPoint p1, IHPoint p2)
        {
            return Math.Sqrt( Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) );
        }

        /// <summary>
        /// генерация граничных точек
        /// </summary>
        private void InitializeBoundary(IHPoint[] basePoints)
        {
            //если точки на границе уже сформированы, то возвращаем их
            if (boundaryPoints != null)
                return;

            double avgDistance = int.MaxValue;
            if (!this.fixedAmount)
                avgDistance = CalculateAvgDistance(basePoints, true);

            List<IHPoint> points = new List<IHPoint>();
            int amountNewPoints = 70;
            //проход по вершинам границы по часовой стрелке
            for (int i = 0; i < boundaryVertex.Length/* - 1*/; i++)
            {
                if (!fixedAmount)
                {
                    amountNewPoints = (int)Math.Floor(
                        this.GetDistance(
                            boundaryVertex[i % boundaryVertex.Length], boundaryVertex[(i + 1) % boundaryVertex.Length]) / avgDistance);
                }

                points.Add(boundaryVertex[i % boundaryVertex.Length]);
                this.AddPointsBetweenVertexes(
                    points, boundaryVertex[i % boundaryVertex.Length], boundaryVertex[(i + 1) % boundaryVertex.Length], amountNewPoints);
            }

            boundaryPoints = points.ToArray();
        }
        
        /// <summary>
        /// добавить новые точки между 2 вершинами
        /// </summary>
        /// <param name="points">список, в который необходимо добавить точки</param>
        void AddPointsBetweenVertexes(List<IHPoint> points, IHPoint v1, IHPoint v2, int amountNewPoints)
        {

            //интервалы смещения по координатно вдоль ребра границы
            double intervalX = (v2.X - v1.X) / amountNewPoints;
            double intervalY = (v2.Y - v1.Y) / amountNewPoints;

            //добавляем новые точки на ребра границы
            for (int j = 1; j < amountNewPoints; j++)
                points.Add(new HPoint(v1.X + intervalX * j, v1.Y + intervalY * j));
        }

        double CalculateAvgDistance(IHPoint[] basePoints, bool inputSortedArray = true)
        {
            if (!inputSortedArray)
                basePoints = new SpecialSorter(basePoints, new Comparator()).GetSortedArray();

            double avgDistance = 0;
            //рассчет среднего расстояния между двумя ближайшими точками
            int amountDistance = basePoints.Length - 1;
            List<string> distances = new List<string>(); //для отладки
            for (int i = 0; i < amountDistance; i++)
            {
                double param = this.GetDistance(basePoints[i], basePoints[i + 1]);
                distances.Add($"{i};{i + 1}: \t{param}");
                avgDistance += param / (basePoints.Length - 1);
            }
            return avgDistance;
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
