using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// Множество границ
    /// </summary>
    public class BoundarySet
    {
        List<Boundary> boundaries = new List<Boundary>();

        private IHPoint[] _allBounaries;
        /// <summary>
        /// Получить все граничные точки
        /// </summary>
        public IHPoint[] AllBoundaryPoints
        {
            get
            {
                if (_allBounaries != null)
                    return _allBounaries;
                List<IHPoint> boundaryPoints = new List<IHPoint>();
                foreach (Boundary boundary in boundaries)
                {
                    boundaryPoints.AddRange(boundary.BoundaryPoints);
                }
                _allBounaries = boundaryPoints.ToArray();
                return _allBounaries;
            }
        }

        /// <summary>
        /// Все множество точек области построения (сетки)
        /// </summary>
        readonly IHPoint[] _basePoints;

        public BoundarySet(IHPoint[] basePoints)
        {
            //this._basePoints = new SpecialSorter(basePoints).GetSortedArray();
            this._basePoints = basePoints;
            var watch = Stopwatch.StartNew();
            avg = CalculateAverageDistance(2);
            string msg = $" Рассчет среднего расстояния между точками ({this._basePoints.Length})шт {watch.Elapsed.TotalSeconds} сек";
            Console.WriteLine(msg);
        }

        double avg = 0;

        public void Add(IHPoint[] vertexes)
        {
            Boundary boundary = new Boundary(vertexes, avg);
            boundaries.Add(boundary);
        }

        //генератор
        public IEnumerator<Boundary> GetEnumerator()
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                yield return boundaries[i];
            }
        }

        /// <summary>
        /// Получить индекс границы, которой принадлежт точка
        /// </summary>
        /// <param name="point"></param>
        /// <returns>-1 если точка не принадлежит границе</returns>
        public int BoundaryIndex(IHPoint point)
        {
            for (int i = 0; i < boundaries.Count; ++i)
            {
                if (Array.IndexOf(boundaries[i].BoundaryPoints, point) != -1)
                    return i;
            }
            return -1;

        }


        double CalculateAverageDistance(int k)
        {
            var distances = new List<double>();
            int n = _basePoints.Length;

            for (int i = 0; i < n; i++)
            {
                var nearestNeighbors = FindNearestNeighbors(_basePoints[i], k);
                foreach (var neighbor in nearestNeighbors)
                {
                    double distance = Math.Sqrt(Math.Pow(neighbor.X - _basePoints[i].X, 2) + Math.Pow(neighbor.Y - _basePoints[i].Y, 2));
                    distances.Add(distance);
                }
            }

            return distances.Count > 0 ? distances.Average() : 0.0;
        }

        IHPoint[] FindNearestNeighbors(IHPoint target, int k)
        {
            //return points
            //    .Where(p => p != target)
            //    .OrderBy(p => Math.Sqrt(Math.Pow(p.X - target.X, 2) + Math.Pow(p.Y - target.Y, 2)))
            //    .Take(k)
            //    .ToArray();

            int n = _basePoints.Length;
            double[] distances = new double[n];
            IHPoint[] neighbors = new IHPoint[k];

            int count = 0;

            // Вычисляем расстояния до всех точек
            for (int i = 0; i < n; i++)
            {
                if (_basePoints[i] != target)
                {
                    distances[i] = Math.Sqrt(Math.Pow(_basePoints[i].X - target.X, 2) + Math.Pow(_basePoints[i].Y - target.Y, 2));
                    if (count < k)
                    {
                        neighbors[count] = _basePoints[i];
                        count++;
                    }
                    else
                    {
                        // Находим максимальное расстояние среди текущих соседей
                        double maxDistance = distances[0];
                        int maxIndex = 0;
                        for (int j = 1; j < k; j++)
                        {
                            if (distances[j] > maxDistance)
                            {
                                maxDistance = distances[j];
                                maxIndex = j;
                            }
                        }

                        // Если текущее расстояние меньше максимального, заменяем
                        if (distances[i] < maxDistance)
                        {
                            neighbors[maxIndex] = _basePoints[i];
                            distances[maxIndex] = distances[i];
                        }
                    }
                }
            }

            return neighbors;
        }
    }
}
