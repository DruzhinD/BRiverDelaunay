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
    public class BoundarySet<T> : List<T> where T : BoundaryCreator
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
                foreach (BoundaryCreator boundary in this)
                {
                    boundaryPoints.AddRange(boundary.GetBoundaryPoints());
                }
                _allBounaries = boundaryPoints.ToArray();
                return _allBounaries;
            }
        }

        IHPoint[] _basePoints;

        public BoundarySet(IHPoint[] basePoints)
        {
            this._basePoints = basePoints;
        }

        public void Add(IHPoint[] vertexes)
        {
            BoundaryCreator boundary = new BoundaryCreator(vertexes, ref _basePoints);
            this.Add(boundary as T);
        }
    }
}
