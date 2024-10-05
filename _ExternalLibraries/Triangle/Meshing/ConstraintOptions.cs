
namespace TriangleNet.Meshing
{
    /// <summary>
    /// Mesh constraint options for polygon triangulation.
    /// Параметры ограничения сетки для триангуляции многоугольника.
    /// </summary>
    public class ConstraintOptions
    {
        // TODO: remove ConstraintOptions.UseRegions
        // ЗАДАЧА: удалить ConstraintOptions.UseRegions

        /// <summary>
        /// Gets or sets a value indicating whether to use regions.
        /// Возвращает или задает значение, указывающее, 
        /// следует ли использовать регионы.
        /// </summary>
        [System.Obsolete("Not used anywhere, will be removed in beta 4.")]
        public bool UseRegions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a Conforming
        /// Delaunay triangulation.
        /// Получает или задает значение, указывающее, следует ли создавать 
        /// триангуляцию Делоне.
        /// </summary>
        public bool ConformingDelaunay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enclose the convex
        /// hull with segments.
        /// Получает или задает значение, указывающее, следует ли заключать 
        /// выпуклую оболочку сегментами.
        /// </summary>
        public bool Convex { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether to suppress boundary
        /// segment splitting.
        /// </summary>
        /// <remarks>
        /// 0 = split segments (default)
        /// 1 = no new vertices on the boundary
        /// 2 = prevent all segment splitting, including internal boundaries
        /// Получает или устанавливает флаг, указывающий, следует ли подавить границу
        /// разделение на сегменты.
        /// </summary>
        /// <примечания>
        /// 0 = разделить сегменты (по умолчанию)
        /// 1 = нет новых вершин на границе
        /// 2 = предотвратить разделение всех сегментов, включая внутренние границы
        /// </remarks>
        public int SegmentSplitting { get; set; }
    }
}
