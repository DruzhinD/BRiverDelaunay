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
        /// <summary>
        /// вершины, образующие границу
        /// </summary>
        public IHPoint[] BoundaryVertex => boundaryVertex;


        IHPoint[] boundaryPoints;
        /// <summary>
        /// точки, принадлежащие границе, включая вершины границы
        /// </summary>
        public IHPoint[] BoundaryPoints
        {
            get
            {
                if (boundaryPoints == null)
                    this.InitializeBoundary();
                return boundaryPoints;
            }
        }

        /// <summary>
        /// Среднее расстояние между точками, мб получено извне (предпочтительно)
        /// </summary>
        double AvgDistance { get; set; } = 0;


        /// <summary>
        /// Создание нового объекта границы
        /// </summary>
        /// <param name="boundary">вершины границы</param>
        /// <param name="avgDistance">среднее расстояние между точками области построения (optional)</param>
        public Boundary(IHPoint[] boundary, double avgDistance = 0)
        {
            this.boundaryVertex = boundary;
            AvgDistance = avgDistance;
        }

        /// <summary>
        /// "Генерирует данные в момент создания. свойства этого класса реализуют прокси (заглушку), кроме данного метода
        /// </summary>
        /// <param name="boundaryVertexes"></param>
        /// <returns></returns>
        [Obsolete()]
        public static Boundary Generate(IHPoint[] boundaryVertexes)
        {
            Boundary boundary = new Boundary(boundaryVertexes);
            boundary.InitializeBoundary();
            return boundary;
        }


        ///<summary>расстояние между точками (длина отрезка)</summary>
        double GetDistance(IHPoint p1, IHPoint p2)
            => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

        /// <summary>
        /// генерация граничных точек
        /// </summary>
        private void InitializeBoundary()
        {
            //если точки на границе уже сформированы, то возвращаем их
            if (boundaryPoints != null)
                return;

            List<IHPoint> points = new List<IHPoint>();
            //проход по вершинам границы по часовой стрелке
            for (int i = 0; i < boundaryVertex.Length/* - 1*/; i++)
            {
                points.Add(boundaryVertex[i % boundaryVertex.Length]);
                this.AddPointsBetweenVertexes(
                    points, boundaryVertex[i % boundaryVertex.Length], boundaryVertex[(i + 1) % boundaryVertex.Length]);
            }

            boundaryPoints = points.ToArray();
        }

        /// <summary>
        /// добавить новые точки между 2 вершинами
        /// </summary>
        /// <param name="points">список, в который необходимо добавить точки</param>
        void AddPointsBetweenVertexes(List<IHPoint> points, IHPoint v1, IHPoint v2)
        {
            //по умолчанию добавляется фикс колиество точек
            int amountNewPoints = 20;
            //если задано среднее расстояние между точками,
            //то на его основе формируем новые граничные точки
            if (AvgDistance > 0)
            {
                int temp = (int)(GetDistance(v1, v2) / AvgDistance);
                if (temp > 0)
                    amountNewPoints = temp;
                else
                    amountNewPoints = 0;
            }

            //интервалы смещения по координатно вдоль ребра границы
            double intervalX = (v2.X - v1.X) / amountNewPoints;
            double intervalY = (v2.Y - v1.Y) / amountNewPoints;

            //добавляем новые точки на ребра границы
            for (int j = 1; j < amountNewPoints; j++)
                points.Add(new HPoint(v1.X + intervalX * j, v1.Y + intervalY * j));
        }
    }
}
