using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TestDelaunayGenerator.Boundary;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// Контейнер-генератор множества областей, представляющих границу
    /// </summary>
    public class BoundaryContainer
    {
        List<BoundaryBase> boundaries = new List<BoundaryBase>();

        public int Count => boundaries.Count;

        private IHPoint[] _allBounaries;
        /// <summary>
        /// Получить все граничные точки
        /// </summary>
        public IHPoint[] AllBoundaryPoints
        {
            get
            {
                if (_allBounaries is null)
                    this.IntializeContainer();
                return _allBounaries;
            }
        }

        /// <summary>
        /// Все множество точек области построения (сетки)
        /// </summary>
        readonly IHPoint[] _basePoints;

        protected readonly GeneratorBase generator;

        /// <summary>
        /// Контейнер для границ области триангуляции
        /// </summary>
        /// <param name="basePoints">точки области триангуляции</param>
        public BoundaryContainer(IHPoint[] basePoints, GeneratorBase generator)
        {
            this._basePoints = basePoints;
            this.generator = generator;
        }


        public void Add(IHPoint[] vertexes)
        {
            BoundaryBase boundary = new BoundaryBase(vertexes, this._basePoints);
            boundaries.Add(boundary);
        }

        //генератор
        public IEnumerator<BoundaryBase> GetEnumerator()
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                yield return boundaries[i];
            }
        }

        /// <summary>
        /// Получить индекс границы, которой принадлежит точка
        /// </summary>
        /// <param name="point"></param>
        /// <returns>-1 если точка не принадлежит границе</returns>
        public int BoundaryIndex(IHPoint point)
        {
            for (int i = 0; i < boundaries.Count; ++i)
            {
                if (Array.IndexOf(boundaries[i].Points, point) != -1)
                    return i;
            }
            return -1;

        }

        protected void IntializeContainer()
        {
            List<IHPoint> boundaryPoints = new List<IHPoint>(2);
            foreach (BoundaryBase boundary in boundaries)
            {
                boundaryPoints.AddRange(
                    boundary.Initialize(this.generator));
            }
            _allBounaries = boundaryPoints.ToArray();
        }
    }
}
