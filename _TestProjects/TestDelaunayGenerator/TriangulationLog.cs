using CommonLib;
using MeshLib;
using TestDelaunayGenerator.Areas;

namespace TestDelaunayGenerator
{
    /// <summary>
    /// Запись для логов триангуляции
    /// </summary>
    public class TriangulationLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="area">исходное область, может содержать границуё</param>
        /// <param name="secondsGenerate">Время генерации триангуляции Делоне</param>
        /// <param name="secondsFilter">Время фильтрации треугольников</param>
        /// <param name="usePointFilter">true - используется только метод заражения треугольников. Фильтрация точек отключена</param>
        public TriangulationLog(AreaBase area, IMesh mesh, double secondsGenerate, double secondsFilter, bool usePointFilter = true)
        {
            PointsBaseBeforeCnt = area.Points.Length;
            HasBoundary = !(area.BoundaryContainer is null);
            PointsBoundaryCnt = 0;
            if (HasBoundary)
            {
                PointsBoundaryCnt = area.BoundaryContainer.AllBoundaryKnots.Length;
                this.BoundaryCount = area.BoundaryContainer.Count;
            }
            AreaType = area.Name;

            this.TrianglesCnt = mesh.CountElements;
            this.PointsBaseAfterCnt = mesh.CountKnots - PointsBoundaryCnt;

            this.SecondsGenerate = secondsGenerate;
            this.SecondsFilter = secondsFilter;
            this.UsePointFilter = usePointFilter;
        }

        /// <summary>
        /// количество исходных узлов
        /// </summary>
        public int PointsBaseBeforeCnt { get; set; }
        
        /// <summary>
        /// количество граничных узлов
        /// </summary>
        public int PointsBoundaryCnt { get; set; } = 0;

        public int BoundaryCount { get; set; }

        /// <summary>
        /// Количество исходных узлов и количество граничных узлов до генерации сетки
        /// </summary>
        public int PointsBeforeCnt => PointsBaseBeforeCnt + PointsBoundaryCnt;

        /// <summary>
        /// Количество узлов, оставшихся после генерации, за вычетом граничных узлов
        /// </summary>
        public int PointsBaseAfterCnt { get; set; }


        /// <summary>
        /// Общее количество узлов, оставшихся после генерации с учетом граничных узлов
        /// </summary>
        public int PointsAfterCnt => PointsBaseAfterCnt + PointsBoundaryCnt;

        /// <summary>
        /// Количество треугольников
        /// </summary>
        public int TrianglesCnt { get; set; }

        /// <summary>
        /// имеется ли граница. true - граница задана
        /// </summary>
        public bool HasBoundary { get; set; } = false;

        /// <summary>
        /// Тип области
        /// </summary>
        public string AreaType { get;set; }

        /// <summary>
        /// Время генерации триангуляции
        /// </summary>
        public double SecondsGenerate { get; set; }

        /// <summary>
        /// Время фильтрации треугольников
        /// </summary>
        public double SecondsFilter { get; set; }

        /// <summary>
        /// Общее затраченное время
        /// </summary>
        public double SecondsTotal => SecondsGenerate + SecondsFilter;


        /// <summary>
        /// True - использовать предварительную фильтрацию исходного множества точек,
        /// оставляя только те, что гарантированно войдут в триангуляцию Делоне
        /// </summary>
        public bool UsePointFilter { get; set; } = true;

        public override string ToString()
        {
            string s = $"Область:{AreaType}; " +
            $"граница:{HasBoundary}; " +
            $"колво граница(шт):{PointsBoundaryCnt}; " +
            $"колво огран областей(шт):{BoundaryCount}; " +
            $"узлы до(шт):{PointsBaseBeforeCnt}; " +
            $"общее до(шт):{PointsBeforeCnt}; " +
            $"узлы после(шт):{PointsBaseAfterCnt}; " +
            $"общее после(шт):{PointsAfterCnt}; " +
            $"треугольники(шт):{TrianglesCnt}; " +
            $"генерация(сек):{SecondsGenerate}; " +
            $"фильтрация(сек):{SecondsFilter}; " +
            $"общее(сек):{SecondsTotal};" +
            $"предварительный фильтр:{UsePointFilter}";
            return s;
        }

        string[] headers = {"область", "граница", "колво граница(шт)", "узлы до(шт)", "узлы после(шт)", "общее после(шт)", "треугольники(шт)", "генерация(сек)", "фильтрация(сек)", "общее(сек)", "предварительный фильтр" };
    }
}
