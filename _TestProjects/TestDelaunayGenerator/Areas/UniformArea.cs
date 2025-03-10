using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Geometry;

namespace TestDelaunayGenerator.Areas
{
    /// <summary>
    /// Равномерное распределение
    /// </summary>
    public class UniformArea : AreaBase
    {
        public UniformArea(int count = 10_000,double valueMin = 0, double valueMax = 1)
            : base(count)
        {
            this.min = valueMin;
            this.max = valueMax;
        }

        public override string Name => "Равномерное распределение";


        double min;
        double max;
        Random rnd = new Random();
        public override void Initialize()
        {
            if (points != null)
                return;
            points = new IHPoint[Count];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new HPoint(Next(), Next());
            }
            IHPoint[] variable;
            if (this.boundaryContainer != null)
                variable = this.boundaryContainer.AllBoundaryKnots;
        }

        double Next()
        {
            return min + rnd.NextDouble() * (max - min);
        }
    }
}
