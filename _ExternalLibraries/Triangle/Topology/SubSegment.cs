// -----------------------------------------------------------------------
// <copyright file="Segment.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.Cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Структура данных подсегмента.
    /// The subsegment data structure.
    /// </summary>
    public class SubSegment : ISegment
    {
        /// <summary>
        /// Хэш для словаря.Будет установлен экземпляром сетки.
        /// Hash for dictionary. Will be set by mesh instance.
        /// </summary>
        internal int hash;

        internal Osub[] subsegs;
        internal Vertex[] vertices;
        internal Otri[] triangles;
        internal int boundary;

        public SubSegment()
        {
            /// <summary>
            /// Four null vertices.
            /// Четыре нулевые вершины.
            /// </summary>
            vertices = new Vertex[4];
            /// <summary>
            /// Set the boundary marker to zero.
            /// Установите маркер границы на ноль.
            /// </summary>
            boundary = 0;
            /// <summary>
            /// Инициализируйте два соседних подсегмента, чтобы они были вездесущими подсегментами.
            /// Initialize the two adjoining subsegments to be the omnipresent subsegment. 
            /// </summary>
            subsegs = new Osub[2];
            /// <summary>
            /// Инициализируйте два соседних треугольника как «космическое пространство».
            /// Initialize the two adjoining triangles to be "outer space."
            /// </summary>
            triangles = new Otri[2];
        }

        #region Public properties

        /// <summary>
        /// Получает идентификатор вершины первой конечной точки.
        /// Gets the first endpoints vertex id.
        /// </summary>
        public int P0
        {
            get { return this.vertices[0].id; }
        }

        /// <summary>
        /// Получает идентификатор вершины второй конечной точки .
        /// Gets the seconds endpoints vertex id.
        /// </summary>
        public int P1
        {
            get { return this.vertices[1].id; }
        }

        /// <summary>
        /// Gets the segment boundary mark.
        /// </summary>
        public int Label
        {
            get { return this.boundary; }
        }

        #endregion

        /// <summary>
        /// Получает метку границы сегмента.
        /// Gets the segments endpoint.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            return this.vertices[index]; // TODO: Check range?
        }

        /// <summary>
        /// Gets an adjoining triangle.
        /// </summary>
        public ITriangle GetTriangle(int index)
        {
            return triangles[index].tri.hash == IMeshNet.DUMMY ? null : triangles[index].tri;
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override string ToString()
        {
            return String.Format("SID {0}", hash);
        }
    }
}
