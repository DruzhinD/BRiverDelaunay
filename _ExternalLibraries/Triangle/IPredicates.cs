// -----------------------------------------------------------------------
// <copyright file="IPredicates.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using TriangleNet.Geometry;
    /// <summary>
    /// Предикаты - манипуляток вернин
    /// </summary>
    public interface IPredicates
    {
        /// <summary>
        /// Против часовой стрелки
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        double CounterClockwise(Point a, Point b, Point c);

        double InCircle(Point a, Point b, Point c, Point p);
        /// <summary>
        /// Найти круговой центр по тем точкам окружности
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dest"></param>
        /// <param name="apex"></param>
        /// <param name="xi"></param>
        /// <param name="eta"></param>
        /// <returns></returns>
        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta);
        /// <summary>
        /// Найти круговой центр по тем точкам окружности
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dest"></param>
        /// <param name="apex"></param>
        /// <param name="xi"></param>
        /// <param name="eta"></param>
        /// <param name="offconstant"></param>
        /// <returns></returns>
        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta,
            double offconstant);
    }
}
