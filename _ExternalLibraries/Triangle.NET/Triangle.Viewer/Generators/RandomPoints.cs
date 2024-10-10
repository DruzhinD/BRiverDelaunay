﻿// -----------------------------------------------------------------------
// <copyright file="RandomPoints.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// Simple random points generator.
    /// </summary>
    public class RandomPoints : BaseGenerator
    {
        public RandomPoints()
        {
            name = "Random Points";
            description = "";
            parameter = 3;

            descriptions[0] = "Number of points:";
            descriptions[1] = "Width:";
            descriptions[2] = "Height:";

            ranges[0] = new int[] { 10, 5000 };
            ranges[1] = new int[] { 10, 200 };
            ranges[2] = new int[] { 10, 200 };
        }

        public override IPolygon Generate(double param0, double param1, double param2)
        {
            int numPoints = GetParamValueInt(0, param0);
            numPoints = (numPoints / 10) * 10;

            if (numPoints < ranges[0][0])
            {
                numPoints = ranges[0][0];
            }

            var input = new Polygon(numPoints);

            int width = GetParamValueInt(1, param1);
            int height = GetParamValueInt(2, param2);

            for (int i = 0; i < numPoints; i++)
            {
                input.Add(new Vertex(Util.Random.NextDouble() * width,
                        Util.Random.NextDouble() * height));
            }

            return input;
        }

        public override void UpdatePolygon(IPolygon geometry)
        {
            //base.UpdatePolygon(geometry);
            int lastPointId = geometry.Points[geometry.Points.Count - 1].ID;
            int width = Util.Random.Next(10, 1000 + geometry.Points.Count);
            int height = Util.Random.Next(10, 5000 + geometry.Points.Count);
            int amountGeneratePoints = 1000;
            for (int i = lastPointId + 1; i < lastPointId + amountGeneratePoints; i++)
            {

                Vertex point = new Vertex(Util.Random.NextDouble() * width,
                        Util.Random.NextDouble() * height);
                point.ID = i;
                geometry.Add(point);
            }
            

        }
    }
}
