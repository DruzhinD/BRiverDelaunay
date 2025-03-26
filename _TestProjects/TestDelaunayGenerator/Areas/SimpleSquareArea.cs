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
                        //new HPoint(0,0),
                        //new HPoint(0,1),
                        //new HPoint(1,1),
                        //new HPoint(1,0),
                        new HPoint(0.32,0.3),
                        new HPoint(0.3,0.7),
                        //new HPoint(0.75,0.7),
                        //new HPoint(0.7,0.3),
                        new HPoint(0.57,0.55),
                        new HPoint(0.45,0.5),

                    };
            BoundaryGenerator = new GeneratorFixed(1);
            var boundary = new IHPoint[]
            {
                        new HPoint(0.4, 0.4),
                        //new HPoint(0.4, 0.5),
                        //new HPoint(0.42, 0.5),
                        new HPoint(0.4, 0.6),
                        new HPoint(0.52,0.52),
                        new HPoint(0.6, 0.4),
                        new HPoint(0.5, 0.45),
            };
            this.AddBoundary(boundary);
            //boundary = new IHPoint[]
            //{
            //            new HPoint(0.3, 0.3),
            //            //new HPoint(0.2, 0.5),
            //            new HPoint(0.3, 0.7),
            //            new HPoint(0.65, 0.7),
            //            new HPoint(0.65, 0.3),
            //};
            //this.AddBoundary(boundary);
        }
    }
}
