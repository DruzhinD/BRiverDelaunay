using CommonLib.Geometry;
using MemLogLib.Diagnostic;
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

        public int Count => boundaries.Count;

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

        /// <summary>
        /// Контейнер для границ области триангуляции
        /// </summary>
        /// <param name="basePoints">точки области триангуляции</param>
        
        public BoundarySet(IHPoint[] basePoints)
        {
            this._basePoints = basePoints;
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
    }
}
