using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Geometry;
using GeometryLib.Vector;

namespace MeshLib.Smoothing
{
    public class SimpleSmoother : ISmoother
    {
        /// <summary>
        /// количество итераций
        /// </summary>
        public int Iteration { get; set; } = 0;
        /// <summary>
        /// Таблица косинусов всех углов заданной сетки. <br/>
        /// KEY: 1-ый int - индекс треугольника в хранилище треугольников сетки; <br/>
        /// 2-ой int - индекс вершины треугольника i=[0:2], которая по совместительству является образующей для угла. <br/>
        /// VALUE: double - значение косинуса угла
        /// </summary>
        Dictionary<(int, int), double> meshCosinusTable;

        /// <summary>
        /// Модифицируемая сетка
        /// </summary>
        TriMesh mesh;

        /// <summary>
        /// Максимальный косинус угла (тупой угол). Признак качества
        /// </summary>
        const double badAngle = 0.9;

        /// <inheritdoc/>
        public void Smooth(IMesh baseMash)
        {
            Iteration++;
            int counter = 0;
            bool rebuild = false;

            //если была передана ссылка на другой объект сетки, то пересчитываем косинусы
            if (mesh == null || mesh.GetHashCode() != baseMash.GetHashCode())
            {
                rebuild = true;
                mesh = (TriMesh)baseMash;
            }
            InitMeshCosinusTable(rebuild);

            //TODO: избавиться от промежуточного хранения новых значений
            Dictionary<(int, int), double> newValues = new Dictionary<(int, int), double>();
            foreach (KeyValuePair<(int, int), double> angleInfo in meshCosinusTable)
            {
                
                //angleInfo = meshCosinusTable.ElementAt(i); //слишком долгий поиск
                int boundFlag = Array.IndexOf<int>(mesh.BoundKnots, (int)IndexGlobalVertex(angleInfo.Key.Item1, angleInfo.Key.Item2));
                //TODO: раздвоить границу для косинусов (для тупых и острых углов)
                //качество треугольника + принадлежность границе
                if (Math.Abs(angleInfo.Value) > badAngle && boundFlag < 0)
                {
                    counter++;
                    //индекс вершины в mesh
                    uint vertexIndex = IndexGlobalVertex(angleInfo.Key.Item1, angleInfo.Key.Item2);

                    //список ключей, ассоциированных с вершиной
                    var tableKeys = AssociatedTriangles(vertexIndex);
                    //перемещаем вершину
                    //IHPoint newCoords = NewVertexCoords(vertexIndex);
                    IHPoint newCoords = InterpolateCoords(tableKeys.Select(x => x.Item1), vertexIndex);

                    mesh.CoordsX[vertexIndex] = newCoords.X;
                    mesh.CoordsY[vertexIndex] = newCoords.Y;

                    //правим косинусы
                    foreach (var key in tableKeys)
                        {
                            List<int> vertexesIndexInTriangle = new List<int>() { 0, 1, 2 };
                            vertexesIndexInTriangle.Remove(key.Item2);
                            double cos = Cosinus(
                                IndexGlobalVertex(key.Item1, vertexesIndexInTriangle[0]),
                                IndexGlobalVertex(key.Item1, key.Item2),
                                IndexGlobalVertex(key.Item1, vertexesIndexInTriangle[1]));

                            //TODO: избавить от этого блока / найти альтернативу
                            try
                            {
                                newValues[key] = cos;

                            }
                            //после изменения AssociatedTriangles не выбрасывается
                            catch (Exception ex)
                            {
                                newValues.Add(key, cos);
                            }
                        }
                }
            }

            foreach (var pair in newValues)
                meshCosinusTable[pair.Key] = pair.Value;
            Console.WriteLine($"Смещенные точки (кол-во): {counter}");
        }

        /// <summary>
        /// Получить индекс вершины в общем массиве индексов всех вершин
        /// </summary>
        /// <returns>глобальный индекс вершины</returns>
        uint IndexGlobalVertex(int triangleId, int internalVertexId) => mesh.AreaElems[triangleId][internalVertexId];

        /// <summary>
        /// Расчет косинуса угла
        /// </summary>
        /// <returns></returns>
        double Cosinus(uint previous, uint center, uint next)
        {
            Vector2 vector1 = new Vector2(mesh.CoordsX[center] - mesh.CoordsX[previous], mesh.CoordsY[center] - mesh.CoordsY[previous]);
            Vector2 vector2 = new Vector2(mesh.CoordsX[center] - mesh.CoordsX[next], mesh.CoordsY[center] - mesh.CoordsY[next]);

            double cos = (vector1.X * vector2.X + vector1.Y * vector2.Y) / (vector1.Length() * vector2.Length()); //math.abs не уместен
            return cos;
        }

        double Sinus(uint previous, uint center, uint next)
        {
            //Vector2 vector1 = new Vector2(mesh.CoordsX[center] - mesh.CoordsX[previous], mesh.CoordsY[center] - mesh.CoordsY[previous]);
            //Vector2 vector2 = new Vector2(mesh.CoordsX[center] - mesh.CoordsX[next], mesh.CoordsY[center] - mesh.CoordsY[next]);

            //var crossRes = vector1.X * vector2.Y - vector1.Y * vector2.X;

            // Векторы AB и AC
            Vector2 vectorAB = new Vector2(mesh.CoordsX[previous] - mesh.CoordsX[center], mesh.CoordsY[previous] - mesh.CoordsY[center]);
            Vector2 vectorAC = new Vector2(mesh.CoordsX[next] - mesh.CoordsX[center], mesh.CoordsY[next] - mesh.CoordsY[center]);

            // Векторное произведение AB и AC
            double crossProduct = vectorAB.X * vectorAC.Y - vectorAB.Y * vectorAC.X;// double crossProduct = abx * acy - aby * acx;

            // Синус угла
            double sine = crossProduct / (vectorAB.Length() * vectorAC.Length());
            return sine;
        }

        /// <summary>
        /// Список ключей в таблице косинусов, содержащих текущую вершину
        /// </summary>
        /// <param name="indexV">Глобальный индекс вершины, содержащейся в сетке</param>
        /// <returns></returns>
        IEnumerable<(int, int)> AssociatedTriangles(uint indexV)
        {
            //id треугольников, в которые входит вершина
            IEnumerable<int> triagnleIds = meshCosinusTable.Keys
                .Where(x => IndexGlobalVertex(x.Item1, x.Item2) == indexV)
                .Select(x => x.Item1);
            //поиск всех ключей, связанных с треугольниками выше
            //TODO оптимизировать. Можно просто возвращать ключи N,0; N,1; N,2 т.к. необходимо пересчитать все углы этих треугольников
            var keys = new List<(int, int)>();

            foreach (int triagnleId in triagnleIds)
                for (int j = 0; j < 3; j++)
                    keys.Add((triagnleId, j));
            //IEnumerable<(int, int)> keys = meshCosinusTable.Keys
            //    .Where(x => triagnleIds.Contains(x.Item1));
            return keys;
        }

        /// <summary>
        /// Получить новые координаты для указанной вершины при сглаживании
        /// </summary>
        IHPoint InterpolateCoords(uint indexV)
        {
            //индексы вершин, входящих в те же треугольники, что и indexV
            ICollection<uint> indexesAssociatedWithVertex = new HashSet<uint>();

            //проход по треугольникам
            foreach (TriElement triangle in mesh.AreaElems)
            {
                //проверка на содержание вершины в треугольнике
                for (int i = 0; i < 3; i++)
                    if (indexV == triangle[i])
                        //добавляем все вершины треугольника во множество ассоциированных вершин
                        for (int j = 0; j < 3; j++)
                            indexesAssociatedWithVertex.Add(triangle[j]);
            }
            //удаляем из множества главную вершины
            indexesAssociatedWithVertex.Remove(indexV);

            double avgX = 0;
            double avgY = 0;
            //расчет новой координаты для переданной вершины
            foreach (uint indexVertex in indexesAssociatedWithVertex)
            {
                avgX += mesh.CoordsX[indexVertex] / indexesAssociatedWithVertex.Count;
                avgY += mesh.CoordsY[indexVertex] / indexesAssociatedWithVertex.Count;
            }

            return new HPoint(avgX, avgY);
        }


        /// <summary>
        /// Получить новые координаты для указанной вершины при помощи интерполяции
        /// </summary>
        /// <param name="associatedTriangles">Список индексов треугольников, в которые данная вершина входит</param>
        /// <param name="targetV">индекс целевой вершины</param>
        IHPoint InterpolateCoords(IEnumerable<int> associatedTriangles, uint targetV)
        {
            //добавляем все вершины треугольников в коллекцию
            var globalVertexes = new HashSet<uint>();
            foreach (var triangleId in associatedTriangles)
                for (int i = 0; i < 3; i++)
                    globalVertexes.Add(IndexGlobalVertex(triangleId, i));
            globalVertexes.Remove(targetV);

            double avgX = 0;
            double avgY = 0;
            //расчет новой координаты для переданной вершины
            foreach (uint indexVertex in globalVertexes)
            {
                avgX += mesh.CoordsX[indexVertex] / globalVertexes.Count;
                avgY += mesh.CoordsY[indexVertex] / globalVertexes.Count;
            }

            return new HPoint(avgX, avgY);
        }


        //составляется верно
        /// <summary>
        /// инициализация таблицы косинусов для всех углов полученной сетки
        /// </summary>
        /// <param name="rebuild">true - нужно заново пересчитать косинусы</param>
        void InitMeshCosinusTable(bool rebuild = false)
        {
            if (meshCosinusTable != null || !rebuild)
                return;
            meshCosinusTable = new Dictionary<(int, int), double>();

            for (int triangleId = 0; triangleId < mesh.AreaElems.Length; triangleId++)
                for (int vertexId = 0; vertexId < 3; vertexId++)
                {
                    TriElement triangle = mesh.AreaElems[triangleId]; //сохраняем треугольник в переменную для упрощения синтаксиса
                    double cos = Cosinus(triangle[vertexId % 3], triangle[(vertexId + 1) % 3], triangle[(vertexId + 2) % 3]);
                    meshCosinusTable.Add((triangleId, (vertexId + 1) % 3), cos);
                }
        }


        /// <summary>
        /// Проверка на выпуклость заданной области
        /// </summary>
        /// <param name="triangleIds">список идентификаторов треугольника</param>
        /// <returns>true - область выпуклая, иначе false</returns>
        bool IsConvex(IEnumerable<int> triangleIds)
        {
            //var sin = 
            return true;
        }
    }
}
