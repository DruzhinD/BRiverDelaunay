using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// Множество границ
    /// </summary>
    public class BoundarySet<T> : List<T> where T : Boundary
    {
        private IHPoint[] _allBounaries;
        /// <summary>
        /// Получить все граничные точки
        /// </summary>
        public IHPoint[] GetAllBoundaryPoints
        {
            get
            {
                if (_allBounaries != null)
                    return _allBounaries;
                List<IHPoint> boundaryPoints = new List<IHPoint>();
                foreach (Boundary boundary in this)
                {
                    boundaryPoints.AddRange(boundary.BoundaryPoints);
                }
                _allBounaries = boundaryPoints.ToArray();
                return _allBounaries;
            }
        }

        IHPoint[] _basePoints;

        public BoundarySet(IHPoint[] basePoints)
        {
            this._basePoints = new SpecialSorter(basePoints, new Comparator()).GetSortedArray();

        }

        public void Add(IHPoint[] vertexes)
        {
            Boundary boundary = Boundary.Generate(this._basePoints, vertexes, true);
            this.Add(boundary as T);
        }
    }
}
