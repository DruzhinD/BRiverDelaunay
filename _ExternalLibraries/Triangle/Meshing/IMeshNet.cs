
namespace TriangleNet.Meshing
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Topology;

    /// <summary>
    /// Mesh interface.
    /// Сетчатый интерфейс.
    /// </summary>
    public interface IMeshNet
    {
        /// <summary>
        /// Gets the vertices of the mesh.
        /// Получает вершины меша.
        /// </summary>
        ICollection<Vertex> Vertices { get; }

        /// <summary>
        /// Gets the edges of the mesh.
        /// Получает края сетки.
        /// </summary>
        IEnumerable<Edge> Edges { get; }

        /// <summary>
        /// Gets the segments (constraint edges) of the mesh.
        /// Получает сегменты (ограничивающие края) сетки.
        /// </summary>
        ICollection<SubSegment> Segments { get; }

        /// <summary>
        /// Gets the triangles of the mesh.
        /// Получает треугольники сетки.
        /// </summary>
        ICollection<Triangle> Triangles { get; }

        /// <summary>
        /// Gets the holes of the mesh.
        /// Получает отверстия сетки.
        /// </summary>
        IList<Point> Holes { get; }

        /// <summary>
        /// Gets the bounds of the mesh.
        /// Получает границы сетки.
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Renumber mesh vertices and triangles.
        /// Перенумеровать вершины и треугольники сетки.
        /// </summary>
        void Renumber();

        /// <summary>
        /// Refine the mesh.
        /// Уточните сетку.
        /// </summary>
        /// <param name="quality">The quality constraints.</param>
        /// <param name="conforming">
        /// A value indicating, if the refined mesh should be Conforming Delaunay.
        /// <param name = "quality"> Ограничения качества. </param>
        /// <param name = "соответствующий">
        /// Значение, показывающее, должна ли уточненная сетка соответствовать Delaunay.
        /// </param>
        void Refine(QualityOptions quality, bool delaunay);
    }
}
