
namespace TriangleNet.Meshing
{
    using TriangleNet.Geometry;

    /// <summary>
    /// Interface for polygon triangulation.
    /// Интерфейс для триангуляции полигонов.
    /// </summary>
    public interface IConstraintMesher
    {
        /// <summary>
        /// Triangulates a polygon.
        /// Триангулирует многоугольник.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>Mesh</returns>
        IMeshNet Triangulate(IPolygon polygon);

        /// <summary>
        /// Triangulates a polygon, applying constraint options.
        /// Триангулирует многоугольник, применяя параметры ограничения.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="options">Constraint options.</param>
        /// <returns>Mesh</returns>
        IMeshNet Triangulate(IPolygon polygon, ConstraintOptions options);
    }
}
