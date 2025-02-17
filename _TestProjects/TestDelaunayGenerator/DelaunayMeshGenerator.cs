﻿//---------------------------------------------------------------------------
//                    ПРОЕКТ  "РУСЛОВЫЕ ПРОЦЕССЫ"
//                         проектировщик:
//                           Потапов И.И.
//---------------------------------------------------------------------------
//                 кодировка : 30.098.2024 Потапов И.И.
//---------------------------------------------------------------------------
namespace TestDelaunayGenerator
{
    using System;
    using System.Linq;
    using CommonLib;
    using CommonLib.Geometry;
    using MeshLib;
    using MemLogLib;
    using GeometryLib.Locators;
    using System.Collections.Generic;
    using GeometryLib.Vector;
    using TestDelaunayGenerator.Boundary;

    /// <summary>
    /// ОО: Делоне генератор выпуклой триангуляции
    /// </summary>
    public class DelaunayMeshGenerator
    {
        /// <summary>
        /// Размер стека для перестройки треугольников по Делоне
        /// </summary>
        private int[] EdgeStack;
        /// <summary>
        ///  Массив индексов вершин треугольника (каждая группа 
        ///  из трех чисел образует треугольник). 
        ///  Обход вершин всех треугольников направлен против часовой стрелки.        
        /// </summary>
        public int[] Triangles;
        /// <summary>
        /// Ссылки индексов ребер треугольника на ребра сопряженных треугольников
        // (или -1 для ребер на выпуклой оболочке). (Ребра диаграмы Вронского)
        /// </summary>
        public int[] HalfEdges;
        /// <summary>
        /// Массив координат входных точек 
        /// </summary>
        public IHPoint[] Points;
        /// <summary>
        /// Маркер узла (внутренний внешний)
        /// </summary>
        private bool[] mark;

        /// <summary>
        /// Размерность хеш пространства
        /// </summary>
        private int hashSize;
        /// <summary>
        /// Массив индексов выпуклой оболочки данных против часовой стрелки
        /// вычисляется по hullNext по окончанию треангуляции
        /// </summary>
        public int[] Hull;
        /// <summary>
        /// Массив индексов выпуклой оболочки данных 
        /// по направлению движения часовой стрелке
        /// </summary>
        private int[] hullPrev;
        /// <summary>
        /// Массив индексов выпуклой оболочки данных  
        /// направлению движения против часовой стрелки
        /// </summary>
        private int[] hullNext;
        /// <summary>
        /// Массив индексов выпуклой оболочки данных против часовой стрелки
        /// </summary>
        private int[] hullTri;
        /// <summary>
        /// хэш-таблица для узлов выпулой оболочки, позволяет "быстро" по псевдо углу 
        /// добовляемого узла определять узел на ближайшей видимой грани оболочки, 
        /// необходимый для добавления в оболочку новых треугольников.
        /// </summary>
        private int[] hullHash;
        /// <summary>
        /// индексы точек отсортированных по растоянию от центра
        /// </summary>
        private int[] ids;
        /// <summary>
        /// Координаты центра триангуляции X
        /// </summary>
        private double cx;
        /// <summary>
        /// Координаты центра триангуляции Y
        /// </summary>
        private double cy;
        /// <summary>
        /// центр триангуляции
        /// </summary>
        protected HPoint pc;

        int i0 = 0;
        int i1 = 0;
        int i2 = 0;
        /// <summary>
        /// счетчик треугольников
        /// </summary>
        private int trianglesLen;
        /// <summary>
        /// Массив входных координат в форме [x0, y0, x1, y1, ....] типа, 
        /// указанного в конструкторе
        /// </summary>
        private double[] coordsX;
        private double[] coordsY;
        /// <summary>
        /// Квадрат расстояния от центра генерации до точки сетки
        /// </summary>
        private double[] dists;

        /// <summary>
        /// условно нулевой узел входа в оболочку
        /// </summary>
        private int hullStart;
        /// <summary>
        /// Количество узлов, образующих выпуклую оболочку
        /// </summary>
        private int CountHullKnots;

        /// <summary>
        /// Внешняя точка, не входящая в сетку
        /// </summary>
        IHPoint externalPoint;

        /// <summary>
        /// ОО: Делоне генератор выпуклой триангуляции
        /// </summary>
        public DelaunayMeshGenerator() { }
        /// <summary>
        /// Содержит группы массивов координат входных точек 
        /// </summary>
        public BoundaryContainer boundarySet;
        /// <summary>
        /// включить множество границ в область построения
        /// </summary>
        void IncludeBoundary()
        {
            IHPoint[] boundary = boundarySet.AllBoundaryPoints;
            int exPointsLength = this.Points.Length;
            Array.Resize(ref this.Points, Points.Length + boundary.Length);
            boundary.CopyTo(this.Points, exPointsLength);
        }

        /// <summary>
        /// Генерация объекта симпл - сетки
        /// </summary>
        /// <param name="DEGUG"></param>
        /// <returns></returns>
        public IMesh CreateMesh(bool DEGUG = false)
        {
            TriMesh mesh = new TriMesh();
            int CountElems = Triangles.Length / 3;
            MEM.Alloc(CountElems, ref mesh.AreaElems);
            List<TriElement> tri = new List<TriElement>();
            for (int i = 0; i < CountElems; i++)
            {
                int i0 = Triangles[3 * i];
                int i1 = Triangles[3 * i + 1];
                int i2 = Triangles[3 * i + 2];
                if (CheckIn(i0, i1, i2) == true)
                {
                    tri.Add(new TriElement((uint)i0, (uint)i1, (uint)i2));
                }
                    //tri.Add(new TriElement((uint)i0, (uint)i1, (uint)i2));
            }

            //сохраняем все треугольники сетки в объект сетки
            mesh.AreaElems = tri.ToArray();
            mesh.CoordsX = this.coordsX;
            mesh.CoordsY = this.coordsY;

            #region формирование граничных точек и линий
            //определение количества точек границы
            int boundaryPointsAmount;
            if (boundarySet != null)
                boundaryPointsAmount = this.boundarySet.AllBoundaryPoints.Length + CountHullKnots;
            else
                boundaryPointsAmount = CountHullKnots;

            //выделение памяти
            MEM.Alloc(boundaryPointsAmount, ref mesh.BoundElems);
            MEM.Alloc(boundaryPointsAmount, ref mesh.BoundElementsMark);
            MEM.Alloc(boundaryPointsAmount, ref mesh.BoundKnots);
            MEM.Alloc(boundaryPointsAmount, ref mesh.BoundKnotsMark);
            int meshIndex = 0;

            //граничные точки и линии, сформированные на основе переданных точек границы (boundarySet)
            if (boundarySet != null)
            {
                //первый индекс граничной точки в массиве
                int notBoundaryOffset = this.Points.Length - boundarySet.AllBoundaryPoints.Length;
                //текущее смещение в общем массиве точек
                int currentOffset = notBoundaryOffset;
                foreach (BoundaryBase boundary in boundarySet)   
                {
                    //индекс массива точек, обозначающий последнюю вершину текущей границы
                    int boundaryLastId = currentOffset + boundary.Length;
                    for (int i = currentOffset; i < boundaryLastId; i++)
                    {
                        int edgeStartId = i; //индекс точки из массива, являющейся началом ребра
                        int edgeEndId = i + 1; //конец ребра
                        //если выход за пределы массива,
                        //то конечной вершиной ребра будет первая граничная точка из текущей границы
                        if (edgeEndId == boundaryLastId)
                            edgeEndId = currentOffset;

                        mesh.BoundElems[meshIndex].Vertex1 = (uint)edgeStartId;
                        mesh.BoundElems[meshIndex].Vertex2 = (uint)edgeEndId;
                        mesh.BoundKnots[meshIndex] = edgeStartId;
                        meshIndex++;
                    }
                    currentOffset += boundary.Length;
                }
            }
            //граничные точки и линии, сформированные на основе всего множества точек сетки (естественным образом)
            if (boundarySet == null || boundarySet.Count % 2 == 0)
                for (int i = 0; i < CountHullKnots; i++)
                {
                    mesh.BoundElems[meshIndex].Vertex1 = (uint)Hull[i];
                    mesh.BoundElems[meshIndex].Vertex2 = (uint)Hull[(i + 1) % CountHullKnots];
                    mesh.BoundElementsMark[meshIndex] = 0;
                    mesh.BoundKnots[meshIndex] = Hull[i];
                    mesh.BoundKnotsMark[meshIndex] = 0;
                    meshIndex++;
                }
            #endregion

            if (DEGUG == true)
                mesh.Print();
            return mesh;
        }
        /// <summary>
        /// Генерация
        /// </summary>
        public void Generator(IHPoint[] points, BoundaryContainer boundSet = null)
        {
            //базовая проверка для множества точек
            if (points is null || points.Length < 3)
                throw new ArgumentOutOfRangeException("Нужно как минимум 3 вершины");
            Points = points;
            //генерация границы
            this.boundarySet = boundSet;
            if (this.boundarySet != null)
                this.IncludeBoundary();

            //выделение памяти
            MEM.Alloc(Points.Length, ref mark, value:true);

            //TODO выяснить резонно ли искать центр тяжести области
            //находим центр тяжести области
            cx = Points.Sum(x => x.X) / (Points.Length);
            cy = Points.Sum(x => x.Y) / (Points.Length);
            pc = new HPoint(cx, cy);

            //внешняя точка
            var maxX = Points.Max(x => x.X);
            externalPoint = new HPoint(maxX * 1.1, cy); //TODO: исправить формирование внешней точки


            // Если контур границы определен,
            //то помечаем точки, которые будут входить в сетку
            if (this.boundarySet != null)
            {
                //выполняем проверку точек вплоть до последней точки из НАЧАЛЬНОГО массива (Points),
                //т.к. массив точек ДОПОЛНЕН массивом граничных точек
                for (var i = 0; i < Points.Length - this.boundarySet.AllBoundaryPoints.Length; i++)
                {
                    // Проверяем, входит ли точка в сетку или же её необходимо исключить
                    mark[i] = InArea(i);
                }
                //очищаем массив от неиспользуемых точек, обрезаем до нужного размера
                FilterPointArray();
            }

            //выделяем память
            int maxTriangles = 2 * Points.Length - 5;
            MEM.Alloc(Points.Length, ref EdgeStack);
            MEM.Alloc(Points.Length, ref coordsX);
            MEM.Alloc(Points.Length, ref coordsY);
            MEM.Alloc(maxTriangles * 3, ref Triangles);
            MEM.Alloc(maxTriangles * 3, ref HalfEdges);
            MEM.Alloc(Points.Length, ref hullPrev);
            MEM.Alloc(Points.Length, ref hullNext);
            MEM.Alloc(Points.Length, ref hullTri);
            MEM.Alloc(Points.Length, ref ids);
            MEM.Alloc(Points.Length, ref dists);
            hashSize = (int)Math.Ceiling(Math.Sqrt(Points.Length)); //размер хэш-пространства
            MEM.Alloc(hashSize, ref hullHash);

            //заполняем массивы хранящие значения X, Y и метку отрисовки точки
            for (var i = 0; i < Points.Length; i++)
            {
                var p = Points[i];
                coordsX[i] = p.X;
                coordsY[i] = p.Y;
                mark[i] = true;
            }

            // Начальное состояние адресации вершин
            for (int i = 0; i < Points.Length; i++)
                ids[i] = i;

            #region поиск начального треугольника
            double minRadius = double.PositiveInfinity;
            var minDist = double.PositiveInfinity;
            #region Попытка упростить формирование начальной оболочки. Пока случаются проблемы
            //функция для получения строкового значения ряда координат по их индексам
            string ToStringCoords(IEnumerable<int> coordsItems)
            {
                IEnumerable<string> stringCoords = coordsItems.Select(
                i => string.Format("{0};{1}", Points[i].X, Points[i].Y));
                return string.Join("|", stringCoords);
            }
            /*
            //TODO искать все 3 точки сразу, в одном цикле относительно центра тяжести области
            // выбираем начальную точку ближе к центру
            for (int i = 0; i < Points.Length; i++)
            {
                if (!mark[i]) continue;
                double curDist = Dist(i);
                //Console.WriteLine(curDist);
                if (curDist < minDist)
                {
                    i2 = i1;
                    i1 = i0;
                    i0 = i;
                    minDist = curDist;
                }
            }
            
            minRadius = Circumradius(i2);

            int[] seedTriangleIds = { i0, i1, i2 };
            Console.WriteLine($"Координаты центра: {cx};{cy}");
            Console.WriteLine($"Новый способ: {string.Join(", ", seedTriangleIds)}");
            Console.WriteLine($"Координаты: {ToStringCoords(seedTriangleIds)}");
            */
            #endregion

            minDist = double.PositiveInfinity;
            for (int i = 0; i < Points.Length; i++)
            {
                //if (mark[i] == false) continue;
                double d = Dist(i);
                if (d < minDist)
                {
                    i0 = i;
                    minDist = d;
                }
            }
            minDist = double.PositiveInfinity;
            // найдите точку, ближайшую к начальной
            for (int i = 0; i < Points.Length; i++)
            {
                if (i == i0) continue;
                //if (mark[i] == false) continue;
                double d = Dist(i0, i);
                if (d < minDist && d > 0)
                {
                    i1 = i;
                    minDist = d;
                }
            }
            // найдите третью точку, которая образует
            // наименьшую окружность с первыми двумя точками
            for (int i = 0; i < Points.Length; i++)
            {
                if (i == i0 || i == i1) continue;
                //if (mark[i] == false) continue;
                double r = Circumradius(i);
                if (r < minRadius)
                {
                    i2 = i;
                    minRadius = r;
                }
            }

            //int[] oldTriangleIds = { i0, i1, i2 };
            //Console.WriteLine($"Старый: {string.Join(", ", oldTriangleIds)}");
            //Console.WriteLine($"Координаты: {ToStringCoords(oldTriangleIds)}");

            if (minRadius == double.PositiveInfinity)
            {
                // Если три точки не найдены! То...
                throw new Exception("Для этих входных данных не существует триангуляции Делоне!");
            }
            // Выберем оринтацию вершин начального треугольника
            if (Orient(i0, i1, i2) == true)
            {
                int i = i1;
                i1 = i2;
                i2 = i;
            }
            #endregion
            //пересчет центра области - центра описанной окружности около начального треугольника
            Circumcenter();
            //расчет расстояний от центра области до каждой из точек в области
            for (var i = 0; i < Points.Length; i++)
            {
                if (mark[i] == false) continue; //пропускает не входящие в область построения
                dists[i] = Dist(i);
            }
            // быстрая сортировка точек по расстоянию от
            // центра окружности исходного треугольника
            Quicksort(ids, dists, 0, Points.Length - 1);

            #region начальная оболочка из первого треугольника
            // стартовый условно нулевой узел входа в оболочку
            hullStart = i0;
            CountHullKnots = 3;
            hullNext[i0] = i1;
            hullNext[i1] = i2;
            hullNext[i2] = i0;

            hullPrev[i2] = i1;
            hullPrev[i0] = i2;
            hullPrev[i1] = i0;

            hullTri[i0] = 0;
            hullTri[i1] = 1;
            hullTri[i2] = 2;

            hullHash[HashKey(i0)] = i0;
            hullHash[HashKey(i1)] = i1;
            hullHash[HashKey(i2)] = i2;
            // счетчик треугольников
            trianglesLen = 0;
            // Добавление 1 треугольника в список треугольников
            AddTriangle(i0, i1, i2, -1, -1, -1);
            #endregion

            #region Поиск выпуклой оболочки и триангуляция
            //TODO можно игнорировать первые 3 узла, т.к. составляют начальную оболочку
            // Поиск выпуклой оболочки и триангуляция
            //узлы, составляющие начальную оболочку (первые 3) не учитываются
            for (var k = 3; k < ids.Length; k++)
            //for (var k = 0; k < ids.Length; k++)
            {
                // добавление текущего k - го узла
                int i = ids[k];

                // поиск  края видимой выпуклой оболочки, используя хэш ребра
                //ближайший узел к текущему на выпуклой оболочке
                int start = 0;
                // поиск близкого узла на выпуклой оболочке 
                // по псевдо углу хеширования
                for (int j = 0; j < hashSize; j++)
                {
                    int key = HashKey(i);
                    start = hullHash[(key + j) % hashSize];
                    if (start != -1 && start != hullNext[start])
                        break;
                }
                start = hullPrev[start];
                int e = start;
                int q = hullNext[e];
                // проверка видимости найденного стартового узла и возможности
                // построения новых треугольников на оболочке
                //true - грань видима для добавляемой точки
                while (Orient(i, e, q) == false)
                {
                    e = q;
                    if (e == start)
                    {
                        // плохой узел 
                        e = int.MaxValue;
                        break;
                    }
                    q = hullNext[e];
                }
                // скорее всего, это почти повторяющаяся точка; пропустите ее
                if (e == int.MaxValue)
                    continue;
                // если e - hullNext[e] - на видимой границе оболочки
                //  добавьте первый треугольник от точки i
                //    hullTri[e]
                //        |
                // -- e ---- hullNext[e] ---
                //     \       /
                //  -1  \     / -1
                //       \   /
                //         i
                
                //индекс первой вершины треугольника в массиве треугольников
                int t = AddTriangle(e, i, hullNext[e], -1, -1, hullTri[e]);
                // рекурсивная перестройки треугольников от точки к точке,
                // пока они не удовлетворят условию Делоне
                hullTri[i] = Legalize(t + 2);
                // добавление треугольника в оболочку
                hullTri[e] = t;
                CountHullKnots++;
                // пройдите вперед по оболочке,
                // добавляя треугольники и переворачивая их рекурсивно
                int nextW = hullNext[e];
                int nextE = hullNext[nextW];
                /// проверка видимой грани (nextW,nextE) оболочки из i точки
                /// при движении вперед по контуру 
                while (Orient(i, nextW, nextE) == true)
                {
                    // если nextW - hullNext[nextW] - на видимой границе оболочки
                    //  добавьте первый треугольник от точки i
                    //
                    //                 hullTri[nextW]
                    //                     |
                    //       ---- nextW ----- hullNext[nextW] --->
                    //               \         /
                    //    hullTri[i]  \       / -1
                    //                 \     /
                    //                  \   /
                    //                    i    
                    // добавить треугольник 
                    t = AddTriangle(nextW, i, nextE, hullTri[i], -1, hullTri[nextW]);
                    //  проверка и перестройка по Делоне
                    hullTri[i] = Legalize(t + 2);
                    // пометить как удаленный узел ущедщий из оболочки
                    hullNext[nextW] = nextW;
                    CountHullKnots--;
                    // следующее ребро оболочки
                    nextW = nextE;
                    nextE = hullNext[nextW];
                }
                // пройдите назад с другой стороны,
                int prewE = e;
                if (prewE == start)
                {
                    int prewW = hullPrev[prewE];
                    while (Orient(i, prewW, prewE) == true)
                    {
                        //  если prewW  - prewE - на видимой границе оболочки
                        //  добавьте первый треугольник от точки i
                        //
                        //                 hullTri[prewW]
                        //                     |
                        //       ----  nextW -----  prewE ---
                        //               \         /
                        //            -1  \       / hullTri[prewE]
                        //                 \     /
                        //                  \   /
                        //                    i    
                        // добавить треугольник 
                        t = AddTriangle(prewW, i, prewE, -1, hullTri[prewE], hullTri[prewW]);
                        //  проверка и перестройка по Делоне
                        Legalize(t + 2);
                        hullTri[prewW] = t;
                        // пометить как удаленный узел ущедщий из оболочки
                        hullNext[prewE] = prewE;
                        CountHullKnots--;
                        // следующее ребро оболочки
                        prewE = prewW;
                        prewW = hullPrev[prewE];
                    }
                }
                // пометить как удаленный
                hullStart = hullPrev[i] = prewE;
                hullNext[prewE] = hullPrev[nextW] = i;
                hullNext[i] = nextW;
                // сохраните два новых ребра в хэш-таблице
                hullHash[HashKey(i)] = i;
                hullHash[HashKey(prewE)] = prewE;
            }
            // Создаем массив граничных узлов оболочки
            Hull = new int[CountHullKnots];
            int s = hullStart;
            for (int i = 0; i < CountHullKnots; i++)
            {
                Hull[i] = s;
                s = hullNext[s];
            }
            #endregion
            // удаляем временные массивы
            hullPrev = hullNext = hullTri = null;
            // обрезка треангуляционных массивов
            // узлы треугольных элементов
            Triangles = Triangles.Take(trianglesLen).ToArray();
            // ребра диаграмы Вронского
            HalfEdges = HalfEdges.Take(trianglesLen).ToArray();
        }

        /// <summary>
        /// Фильтрует исходное множество точек, оставляя только те,
        /// что входят в область построения. <br/>
        /// Обрезает массив точек и связанные с ним до необходимого размера
        /// </summary>
        void FilterPointArray()
        {
            //количество точек области, входящих в область триангуляции
            int markedPointsAmount = mark.Count(x => x is true);
            
            //следующий индекс для перезаписи в массиве
            int curNewPointIndex = 0;
            //сужаем исходный массив до размера, необходимого лишь множеству точек из области построения
            for (int i = 0; i < mark.Length; i++)
            {
                if (mark[i])
                {
                    Points[curNewPointIndex] = //новое положение точки
                        Points[i]; //старое положение точки
                    curNewPointIndex++;
                }
            }

            //обрезаем исходный массив точек до необходимого размера
            Array.Resize(ref Points, markedPointsAmount);
            
            //TODO mark не нужен, ибо в нем все элементы True
            //перезаполняем mark
            MEM.Alloc<bool>(Points.Length, ref mark, value: true);
        }

        #region CreationLogic
        /// <summary>
        /// знак верктоного произведения построенного на касательных к двум граням. <br/>
        /// Используется для проверки угла на выпуклость, т.е. true - угол < 180
        /// </summary>
        /// <param name="i"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        bool Orient(int i, int q, int r)
        {
            return (coordsY[q] - coordsY[i]) * (coordsX[r] - coordsX[q])
                 - (coordsX[q] - coordsX[i]) * (coordsY[r] - coordsY[q]) < 0;
        }
        /// <summary>
        /// рекурсивная перестройки треугольников от точки к точке,
        /// пока они не удовлетворят условию Делоне 
        /// </summary>
        /// <param name="EdgeA_ID">индекс 3-ей вершины треугольника в массиве Triangles</param>
        /// <returns>индекс 2-ой (средней) вершины треугольника</returns>
        private int Legalize(int EdgeA_ID)
        {
            var i = 0;
            int ar;

            // recursion eliminated with EdgeA_ID fixed-size stack
            // рекурсия устранена с помощью стека фиксированного размера
            while (true)
            {
                var EdgeB_ID = HalfEdges[EdgeA_ID];

                // Если пара треугольников не удовлетворяет условию Делоне
                // (p1 находится внутри описанной окружности [p0, pl, pr]),
                // переверните их против часовой стрелки.
                // Выполните ту же проверку рекурсивно для новой пары
                // треугольников
                //                                    triA
                //            pl                       pl
                //           /||\                     /  \
                //        al/ || \bl               al/    \EdgeA_ID
                //         /  ||  \                 /      \
                //    EdgeA_ID|| EdgeB_ID  flip    /___ar___\
                //      p0\   ||   /p1      =>   p0\---bl---/p1
                //         \  ||  /                 \      /
                //        ar\ || /br         EdgeB_ID\    /br
                //           \||/                     \  /
                //            pr                       pr
                //                                    triB

                // адрес - смешение для 1 треугольника (1-ый индекс в треугольнике)
                int triA_ID = EdgeA_ID - EdgeA_ID % 3;
                ar = triA_ID + (EdgeA_ID + 2) % 3;

                //если смежный треугольник не был найден (т.е. -1), то достаем следующий из стека
                if (EdgeB_ID == -1)
                {
                    // граница выпуклой оболочки 
                    if (i == 0)
                        break;
                    EdgeA_ID = EdgeStack[--i];
                    continue;
                }

                int al = triA_ID + (EdgeA_ID + 1) % 3;
                // адрес - смешение для 2 треугольника
                int triB_ID = EdgeB_ID - EdgeB_ID % 3;
                int bl = triB_ID + (EdgeB_ID + 2) % 3;

                //индексы вершин двух смежных треугольников
                int p0 = Triangles[ar];
                int pr = Triangles[EdgeA_ID];
                int pl = Triangles[al];
                int p1 = Triangles[bl]; //вершина смежного треугольника

                bool illegal = InCircle(p0, pr, pl, p1);
                if (illegal)
                {
                    Triangles[EdgeA_ID] = p1;
                    Triangles[EdgeB_ID] = p0;
                    int hbl = HalfEdges[bl];
                    // ребро поменяно местами на другой стороне оболочки (редко);
                    // исправить ссылку ребра смежного треугольника
                    if (hbl == -1)
                    {
                        int e = hullStart;
                        do
                        {
                            if (hullTri[e] == bl)
                            {
                                hullTri[e] = EdgeA_ID;
                                break;
                            }
                            e = hullPrev[e];
                        }
                        while (e != hullStart);
                    }
                    Link(EdgeA_ID, hbl);
                    Link(EdgeB_ID, HalfEdges[ar]);
                    Link(ar, bl);
                    // не беспокойтесь о достижении предела: это может
                    // произойти только при крайне вырожденном вводе
                    if (i < EdgeStack.Length)
                    {
                        //помещаем середину второго треугольника полученного при флипе
                        EdgeStack[i++] = triB_ID + (EdgeB_ID + 1) % 3;
                    }
                    else
                    {
                        Console.WriteLine("Переполнение стека при проверке Делоне" +
                            " для добавленных треугольников!");
                        break;
                    }
                }
                else
                {
                    if (i == 0)
                        break;
                    EdgeA_ID = EdgeStack[--i];
                }
            }
            return ar;
        }
        /// <summary>
        /// принадлежность узла кругу проведенному через три точки
        /// </summary>
        /// <param name="i">V1</param>
        /// <param name="j">V2</param>
        /// <param name="k">V3</param>
        /// <param name="n">проверяемый узел, не должен входить в окружность</param>
        /// <returns></returns>
        private bool InCircle(int i, int j, int k, int n)
        {
            var dx = coordsX[i] - coordsX[n];
            var dy = coordsY[i] - coordsY[n];
            var ex = coordsX[j] - coordsX[n];
            var ey = coordsY[j] - coordsY[n];
            var fx = coordsX[k] - coordsX[n];
            var fy = coordsY[k] - coordsY[n];

            var ap = dx * dx + dy * dy;
            var bp = ex * ex + ey * ey;
            var cp = fx * fx + fy * fy;

            return dx * (ey * cp - bp * fy) -
                   dy * (ex * cp - bp * fx) +
                   ap * (ex * fy - ey * fx) < 0;
        }
        /// <summary>
        /// Добавление треугольника в список треугольников
        /// </summary>
        /// <param name="i0">индекс вершины 0</param>
        /// <param name="i1">индекс вершины 1</param>
        /// <param name="i2">индекс вершины 2</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>возвращает адрес смещения для нового треугольника</returns>
        private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
        {
            var triangleID = trianglesLen;
            Triangles[triangleID] = i0;
            Triangles[triangleID + 1] = i1;
            Triangles[triangleID + 2] = i2;

            Link(triangleID, a);
            Link(triangleID + 1, b);
            Link(triangleID + 2, c);

            trianglesLen += 3;
            return triangleID;
        }
        private void Link(int EdgesID, int b)
        {
            HalfEdges[EdgesID] = b;
            if (b != -1)
                HalfEdges[b] = EdgesID;
        }
        /// <summary>
        /// Получение хеш индекса через псевдо угол точки относительно 
        /// начального центра триангуляции
        /// </summary>
        /// <param name="idx">индекс точки в исходном массиве</param>
        /// <returns></returns>
        private int HashKey(int idx)
        {
            //разность координат между текущей точкой и центром триангуляции требуется для того,
            //чтобы принять центр триангуляции за центр координат
            return (int)(PseudoAngle(coordsX[idx] - cx,
                coordsY[idx] - cy) * hashSize) % hashSize;
        }
        /// <summary>
        /// определение квадрата радиуса окружности проходящей через 3 точки
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private double Circumradius(int i)
        {
            double dx = coordsX[i1] - coordsX[i0];
            double dy = coordsY[i1] - coordsY[i0];
            double ex = coordsX[i] - coordsX[i0];
            double ey = coordsY[i] - coordsY[i0];
            double bl = dx * dx + dy * dy;
            double cl = ex * ex + ey * ey;
            double d = 0.5 / (dx * ey - dy * ex);
            double x = (ey * bl - dy * cl) * d;
            double y = (dx * cl - ex * bl) * d;
            return x * x + y * y;
        }
        /// <summary>
        /// Пересчет начальной точки области - центра описанной окружности около начальной оболочки,
        /// т.е. начально треугольника
        /// </summary>
        private void Circumcenter()
        {
            //координаты вершин начального треугольника
            double ax = coordsX[i0];
            double ay = coordsY[i0];
            double dx = coordsX[i1] - coordsX[i0];
            double dy = coordsY[i1] - coordsY[i0];
            double ex = coordsX[i2] - coordsX[i0];
            double ey = coordsY[i2] - coordsY[i0];

            //расчет центра описанной окружности
            double bl = dx * dx + dy * dy;
            double cl = ex * ex + ey * ey;
            double d = 0.5 / (dx * ey - dy * ex);
            cx = ax + (ey * bl - dy * cl) * d;
            cy = ay + (dx * cl - ex * bl) * d;
            pc = new HPoint(cx, cy);
        }
        /// <summary>
        /// Вычисление псевдо угола точки 
        /// </summary>
        /// <param name="dx">отклонение точки от центра координат по оси Х</param>
        /// <param name="dy">отклонение точки от центра координат по оси Y</param>
        /// <returns>псевно угол (упрощенная альтернатива полярному углу)</returns>
        private static double PseudoAngle(double dx, double dy)
        {
            var p = dx / (Math.Abs(dx) + Math.Abs(dy));
            return (dy > 0 ? 3 - p : 1 + p) / 4; // [0..1]
        }
        /// <summary>
        /// быстрая сортировка точек по расстоянию от центра окружности исходного треугольника
        /// </summary>
        /// <param name="ids">индексы сортируемых точек</param>
        /// <param name="dists">расстояния от центра до сортируемой точки</param>
        /// <param name="left">начальный номер узла сортируемых массивов</param>
        /// <param name="right">конечный номер узла сортируемых массивов</param>
        private static void Quicksort(int[] ids, double[] dists, int left, int right)
        {
            if (right - left <= 20)
            {
                for (var i = left + 1; i <= right; i++)
                {
                    var temp = ids[i];
                    var tempDist = dists[temp];
                    var j = i - 1;
                    while (j >= left && dists[ids[j]] > tempDist) ids[j + 1] = ids[j--];
                    ids[j + 1] = temp;
                }
            }
            else
            {
                var median = (left + right) >> 1;
                var i = left + 1;
                var j = right;
                Swap(ids, median, i);
                if (dists[ids[left]] > dists[ids[right]]) Swap(ids, left, right);
                if (dists[ids[i]] > dists[ids[right]]) Swap(ids, i, right);
                if (dists[ids[left]] > dists[ids[i]]) Swap(ids, left, i);

                var temp = ids[i];
                var tempDist = dists[temp];
                while (true)
                {
                    do i++; while (dists[ids[i]] < tempDist);
                    do j--; while (dists[ids[j]] > tempDist);
                    if (j < i) break;
                    Swap(ids, i, j);
                }
                ids[left + 1] = ids[j];
                ids[j] = temp;

                if (right - i + 1 >= j - left)
                {
                    Quicksort(ids, dists, i, right);
                    Quicksort(ids, dists, left, j - 1);
                }
                else
                {
                    Quicksort(ids, dists, left, j - 1);
                    Quicksort(ids, dists, i, right);
                }
            }
        }
        /// <summary>
        /// Поменять местами элементы в массиве (сделать свап, смену)
        /// </summary>
        /// <param name="arr">массив с элементами</param>
        /// <param name="i">индекс 1 элемента</param>
        /// <param name="j">индекс 2 элемента</param>
        private static void Swap(int[] arr, int i, int j)
        {
            var tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }

        /// <summary>
        /// Квадрат расстояния между точками по указанным индексам
        /// </summary>
        /// <param name="i">индекс 1 точки</param>
        /// <param name="j">индекс 2 точки</param>
        private double Dist(int i, int j)
        {
            var dx = coordsX[i] - coordsX[j];
            var dy = coordsY[i] - coordsY[j];
            return dx * dx + dy * dy;
        }
        /// <summary>
        /// Квадрат расстояния от центра области до точки с указанным индексом
        /// </summary>
        /// <param name="j">индекс точки</param>
        private double Dist(int j)
        {
            var dx = cx - coordsX[j];
            var dy = cy - coordsY[j];
            return dx * dx + dy * dy;
        }
        /// <summary>
        /// Точка принадлежит области
        /// </summary>
        /// <param name="i">индекс точки в массиве</param>
        /// <returns>True - точка принадлежит области</returns>
        private bool InArea(int i)
        {
            //передаем конкретную точку
            return InArea((HPoint)Points[i]);
        }
        private bool InArea(HPoint Point)
        {
            //количество пересечений с границей
            int crossCount = 0;
            //метод - хелпер, помогающий отрисовать невыпуклый контур
            //в цикле подсчитывается количество пересечений с границей области
            foreach (BoundaryBase boundary in boundarySet)
                for (int k = 0; k < boundary.Vertexes.Length; k++)
                {
                    if (CrossLine.IsCrossing(
                        (HPoint)boundary.Vertexes[k],
                        (HPoint)boundary.Vertexes[(k + 1) % boundary.Vertexes.Length],
                         (HPoint)externalPoint,
                         Point) == true)
                        crossCount += 1;
                }
            return (crossCount % 2 == 1);
        }

        /// <summary>
        /// Проверка пересечения отрезка, обрзованного двумя точками с элементами границы
        /// </summary>
        /// <returns>True - линия пересекает границу</returns>
        bool InArea(HPoint p1, HPoint p2)
        {
            foreach (BoundaryBase boundary in boundarySet)
                for (int k = 0; k < boundary.Length; k++)
                {
                    if (CrossLine.AreLinesIntersecting(
                        (HPoint)boundary[k],
                        (HPoint)boundary[(k + 1) % boundary.Length],
                         p1,
                         p2,
                         false) == true)
                        return true;
                }
            return false;

        }
        /// <summary>
        /// Принадлежит ли треугольник невыпуклой области <br/>
        /// помечает треугольники, которые принадлежат невыпуклой области <br/>
        /// args: индексы вершин треугольника
        /// </summary>
        /// <returns>True - точка принадлежит области</returns>
        private bool CheckIn(int i, int j, int k)
        {
            //i == 10121 || i == 10119 || i == 1462 //good
            //i == 10138 || i == 10137 || i == 10102
            //если граница не определена, то помечаем точку, как входящую в сетку
            if (boundarySet == null) return true;

            //вершины треугольника являются граничными узлами
            int indexMaxNotBoundaryPoint = Points.Length - boundarySet.AllBoundaryPoints.Length - 1;
            if (i > indexMaxNotBoundaryPoint && j > indexMaxNotBoundaryPoint && k > indexMaxNotBoundaryPoint)
            {
                double ctx = (coordsX[i] + coordsX[j] + coordsX[k]) / 3;
                double cty = (coordsY[i] + coordsY[j] + coordsY[k]) / 3;
                HPoint ctri = new HPoint(ctx, cty);
                return InArea(ctri);
            }
            //хотя бы 1 вершина треугольника не является граничным узлом
            else
                //return (mark[i] && mark[j] && mark[k]);
                return true;


            //проверка на пересечение ребер треугольника с границей
            /*
            int[] pointIds = new int[]
            {
                i, j, k
            };
            for (int d = 0; d < pointIds.Length; d++)
            {
                int pId = pointIds[d];
                int next = pointIds[(d + 1) % pointIds.Length];
                HPoint p1 = new HPoint(coordsX[pId], coordsY[pId]);
                HPoint p2 = new HPoint(coordsX[next], coordsY[next]);
                if (InArea(p1, p2))
                    return false;
            }
            return true;
            */
        }

        #endregion CreationLogic
    }
}