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
            double[] dxx = {0.00000001, 0.00000005, 0.00000002, 0.00000006, 0.00000002,
                            0.00000007, 0.00000003, 0.00000001, 0.00000004, 0.00000009,
                            0.00000000, 0.00000003, 0.00000006, 0.00000004, 0.00000008 };
            double[] dyy = { 0.00000005, 0.00000002, 0.00000006, 0.00000002, 0.00000004,
                             0.00000007, 0.00000003, 0.00000001, 0.00000001, 0.00000004,
                             0.00000009, 0.00000000, 0.00000003, 0.00000006,  0.00000008 };
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
