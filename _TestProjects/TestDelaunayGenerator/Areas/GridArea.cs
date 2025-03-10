using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Geometry;

namespace TestDelaunayGenerator.Areas
{
    /// <summary>
    /// Генератор обычной квадратной сетки
    /// </summary>
    public class GridArea : AreaBase
    {

        public override string Name => "Сетка";

        double maxValue;

        public GridArea(int count = 10_000, double maxValue = 1)
            :base(count)
        {
            this.maxValue = maxValue;
        }

        public override void Initialize()
        {
            if (points != null)
                return;

            // массивы для псевдослучайного микро смещения координат узлов
            double[] dxx = {0.0000001, 0.0000005, 0.0000002, 0.0000006, 0.0000002,
                            0.0000007, 0.0000003, 0.0000001, 0.0000004, 0.0000009,
                            0.0000000, 0.0000003, 0.0000006, 0.0000004, 0.0000008 };
            double[] dyy = { 0.0000005, 0.0000002, 0.0000006, 0.0000002, 0.0000004,
                             0.0000007, 0.0000003, 0.0000001, 0.0000001, 0.0000004,
                             0.0000009, 0.0000000, 0.0000003, 0.0000006,  0.0000008 };
            int idd = 0;

            int sqrtCnt = (int)Math.Sqrt(Count);
            double coordOffset = maxValue / sqrtCnt;
            points = new IHPoint[sqrtCnt*sqrtCnt];
            int indexer = 0;
            for (int i = 0; i < sqrtCnt; i++)
                for (int j = 0; j < sqrtCnt; j++)
                {
                    points[indexer++] = new HPoint(i * coordOffset + dxx[idd], j * coordOffset + dyy[idd]);
                    idd++;
                    idd = idd % dxx.Length;
                }

            IHPoint[] variable;
            if (this.boundaryContainer != null)
                variable = this.boundaryContainer.AllBoundaryKnots;
        }
    }
}
