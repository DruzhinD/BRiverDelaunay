using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using CommonLib.Geometry;
using MeshLib;

namespace TestDelaunayGenerator.Smoothing
{
    public class SimpleSmoother : ISmoother
    {
        /// <summary>
        /// Модифицируемая сетка
        /// </summary>
        TriMesh mesh;

        /// <summary>
        /// Предельное значение, равное отношению длины стороны треугольника к его периметру. Признак качества 
        /// </summary>
        const double badValue = 0.15;

        /// <inheritdoc/>
        public void Smooth(IMesh baseMash)
        {
            mesh = (TriMesh)baseMash;

            //итеративно проверяем качество всех треугольников
            //for (int i = 0; i < triangles.Length; i++)
            foreach (var triangle in mesh.AreaElems)
            {
                int index = DefaultOrBadVertexIndex(triangle);
                if (index < 0)
                    continue;
                IHPoint newCoords = NewVertexCoords(triangle[index]);
                mesh.CoordsX[triangle[index]] = newCoords.X;
                mesh.CoordsY[triangle[index]] = newCoords.Y;
            }
        }

        /// <summary>
        /// Проверяет треугольник на соответствие условиям качества
        /// </summary>
        /// <returns>если треугольник хороший, то возврат: -1; иначе индекс вершины треугольника, если рассматривать <see cref="TriElement"/> как массив</returns>
        int DefaultOrBadVertexIndex(in TriElement triangle)
        {
            double v1v2 = VectorLength(triangle[0], triangle[1]);
            double v1v3 = VectorLength(triangle[0], triangle[2]);
            double v2v3 = VectorLength(triangle[1], triangle[2]);

            double perimeter = v1v2 + v1v3 + v2v3;
            if (v1v2 / perimeter < badValue)
                return 2;
            else if (v1v3 / perimeter < badValue)
                return 1;
            else if (v2v3 / perimeter < badValue)
                return 0;

            return -1;
        }

        /// <summary>
        /// Длина вектора, построенного по переданным индексам вершин
        /// </summary>
        double VectorLength(uint indexV1, uint indexV2)
        {
            IHPoint p1 = new HPoint(mesh.CoordsX[indexV1], mesh.CoordsY[indexV1]);
            IHPoint p2 = new HPoint(mesh.CoordsX[indexV2], mesh.CoordsY[indexV2]);
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }


        /// <summary>
        /// Получить новые координаты для указанной вершины при сглаживании
        /// </summary>
        IHPoint NewVertexCoords(uint indexV)
        {
            //индексы вершин, входящих в те же треугольники, что и indexV
            ICollection<uint> indexesAssociatedWithVertes = new HashSet<uint>();

            //проход по треугольникам
            foreach (TriElement triangle in mesh.AreaElems)
            {
                //проверка на содержание вершины в треугольнике
                for (int i = 0; i < 3;  i++)
                    if (indexV == triangle[i])
                        //добавляем все вершины треугольника во множество ассоциированных вершин
                        for (int j = 0; j < 3; j++)
                            indexesAssociatedWithVertes.Add(triangle[j]);
            }
            //удаляем из множества главную вершины
            indexesAssociatedWithVertes.Remove(indexV);

            double avgX = 0;
            double avgY = 0;
            //расчет новой координаты для переданной вершины
            foreach (uint indexVertex in indexesAssociatedWithVertes)
            {
                avgX += mesh.CoordsX[indexVertex] / indexesAssociatedWithVertes.Count;
                avgY += mesh.CoordsY[indexVertex] / indexesAssociatedWithVertes.Count;
            }

            return new HPoint(avgX, avgY);
        }
    }
}
