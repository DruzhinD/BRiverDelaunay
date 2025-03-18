using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelaunayGenerator.Boundary;

namespace TestDelaunayGenerator.Areas
{
    public class SimpleSquareArea : AreaBase
    {
        public override string Name => "Простой квадрат";

        public override void Initialize()
        {
            if (!(this.points is null))
                return;

            points = new IHPoint[]
                    {
                        new HPoint(0,0),
                        new HPoint(0,1),
                        new HPoint(1,1),
                        new HPoint(1,0),
                        new HPoint(0.32,0.3),
                        new HPoint(0.3,0.7),
                        new HPoint(0.75,0.7),
                        new HPoint(0.7,0.3),
                        new HPoint(0.57,0.55),
                        new HPoint(0.45,0.5),

                        //new HPoint(0.5, 0.7),

                    };
            BoundaryGenerator = new GeneratorFixed(0);
            var boundary = new IHPoint[]
            {
                        new HPoint(0.4, 0.4),
                        new HPoint(0.4, 0.6),
                        new HPoint(0.52,0.52),
                        new HPoint(0.6, 0.4),
            };
            this.AddBoundary(boundary);
            boundary = new IHPoint[]
            {
                        new HPoint(-0.1, -0.1),
                        new HPoint(0.2, 0.5),
                        new HPoint(-0.1, 1.1),
                        new HPoint(1.1, 1.1),
                        new HPoint(1.1, -0.1),
            };
            this.AddBoundary(boundary);
        }
    }
}
