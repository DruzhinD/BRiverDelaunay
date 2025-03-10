using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Geometry;

namespace TestDelaunayGenerator.Areas
{
    /// <summary>
    /// Область с нормальным распределением в форме квадрата
    /// </summary>
    public class GaussArea : AreaBase
    {
        public GaussArea(int count = 10_000, double mean = 100, double stdDev = 10)
            : base(count)
        {
            this.mean = mean;
            this.stdDev = stdDev;
        }

        /// <summary>
        /// среднее значение
        /// </summary>
        double mean = 180;
        /// <summary>
        /// стандартное отклонение
        /// </summary>
        double stdDev = 10;
        public override string Name => "Нормальное распределение";

        public override void Initialize()
        {
            if (points != null)
                return;
            points = new IHPoint[Count];

            for (int i = 0; i < points.Length; i++)
                points[i] = new HPoint(Next(), Next());

            IHPoint[] variable;
            if (this.boundaryContainer != null)
                variable = this.boundaryContainer.AllBoundaryKnots;
        }

        /// <summary>
        /// Предыдущее значение кешировано или нет
        /// </summary>
        bool hasCashedValue = false;
        /// <summary>
        /// Предыдущее кешированное значение
        /// </summary>
        double cachedValue = 0;
        Random rnd = new Random();

        /// <summary>
        /// Генерация следующего числа из нормального распределения
        /// </summary>
        /// <returns></returns>
        double Next()
        {
            if (hasCashedValue)
            {
                hasCashedValue = false;
                return cachedValue * stdDev + mean;
            }

            double u1 = rnd.NextDouble();
            double u2 = rnd.NextDouble();

            double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
            double z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

            cachedValue = z1;
            hasCashedValue = true;

            return z0 * stdDev + mean;
        }
    }
}
