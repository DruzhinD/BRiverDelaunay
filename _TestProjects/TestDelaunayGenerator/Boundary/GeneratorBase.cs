using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelaunayGenerator;

namespace TestDelaunayGenerator.Boundary
{
    public abstract class GeneratorBase
    {
        public abstract IHPoint[] Generate(BoundaryBase boundary);
    }
}
