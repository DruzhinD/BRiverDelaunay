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

        public BoundaryBase this[int boundId]
        {
            get
            {
                if (boundId > boundaries.Count - 1)
                    throw new ArgumentException($"{nameof(boundId)} вышел за пределы индексации.");
                return boundaries[boundId];
            }
        }

        /// <summary>
        /// Количество границ
        /// </summary>
        public int Count => boundaries.Count;

        /// <summary>
        /// Смещение по количеству узлов в общем массиве узлов для конкретной границы. <br/>
        /// Для первой границы смещение будет 0, для 2-ой границы смещение будет 0 + количество узлов в первой границе и т.д.
        /// </summary>
        /// <param name="boundId"></param>
        /// <returns></returns>
        public int GetBoundaryOffset(int boundId)
        {
            if (boundId > boundaries.Count - 1 || boundId < 0)
                throw new ArgumentException($"{nameof(boundId)} вышел за пределы индексации.");

            int offset = 0;
            for (int i = 0; i < boundId; i++)
            {
                offset += boundaries[i].Length;
            }
            return offset;
        }

        /// <summary>
        /// Все граничные узлы
        /// </summary>
        private IHPoint[] _allBounaryKnots;
        /// <summary>
        /// Получить все граничные точки
        /// </summary>
        public IHPoint[] AllBoundaryKnots
        {
            get
            {
                if (_allBounaryKnots is null)
                    this.IntializeContainer();
                return _allBounaryKnots;
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
            _allBounaryKnots = boundaryPoints.ToArray();
        }
    }
}
