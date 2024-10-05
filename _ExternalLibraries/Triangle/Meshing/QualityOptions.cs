
namespace TriangleNet.Meshing
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Mesh constraint options for quality triangulation.
    /// Параметры ограничения сетки для качественной триангуляции.
    /// </summary>
    public class QualityOptions
    {
        /// <summary>
        /// Gets or sets a maximum angle constraint.
        /// Получает или задает ограничение максимального угла.
        /// </summary>
        public double MaximumAngle { get; set; }

        /// <summary>
        /// Gets or sets a minimum angle constraint.
        /// Получает или задает ограничение минимального угла.
        /// </summary>
        public double MinimumAngle { get; set; }

        /// <summary>
        /// Gets or sets a maximum triangle area constraint.
        /// Получает или задает ограничение максимальной площади треугольника.
        /// </summary>
        public double MaximumArea { get; set; }

        /// <summary>
        /// Gets or sets a user-defined triangle constraint.
        /// Получает или задает определяемое пользователем ограничение треугольника.
        /// </summary>
        /// <remarks>
        /// The test function will be called for each triangle in the mesh. The
        /// second argument is the area of the triangle tested. If the function
        /// returns true, the triangle is considered bad and will be refined.
        /// Тестовая функция будет вызываться для каждого треугольника в сетке. В
        /// второй аргумент - это площадь тестируемого треугольника. Если функция
        /// возвращает истину, треугольник считается плохим и будет уточнен.
        /// </remarks>
        public Func<ITriangle, double, bool> UserTest { get; set; }

        /// <summary>
        /// Gets or sets an area constraint per triangle.
        /// Получает или задает ограничение площади для треугольника.
        /// </summary>
        /// <remarks>
        /// If this flag is set to true, the <see cref="ITriangle.Area"/> value will
        /// be used to check if a triangle needs refinement.
        /// <примечания>
        /// Если этот флаг установлен в значение true, 
        /// значение <see cref = "ITriangle.Area" /> будет
        /// можно использовать для проверки необходимости уточнения треугольника.

        /// </remarks>
        public bool VariableArea { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of Steiner points to be inserted into the mesh.
        /// Получает или задает максимальное количество точек Штейнера для вставки в сетку.
        /// </summary>
        /// <remarks>
        /// If the value is 0 (default), an unknown number of Steiner points may be inserted
        /// to meet the other quality constraints.
        /// <примечания>
        /// Если значение равно 0 (по умолчанию), может быть вставлено неизвестное количество 
        /// точек Штейнера для соответствия другим ограничениям качества.
        /// </remarks>
        public int SteinerPoints { get; set; }
    }
}
