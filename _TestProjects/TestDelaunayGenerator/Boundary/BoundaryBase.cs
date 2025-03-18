using CommonLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelaunayGenerator.Boundary
{
    /// <summary>
    /// Конкретная область границы
    /// </summary>
    public class BoundaryBase
    {
        protected IHPoint[] points;
        /// <summary>
        /// Точки, образующие границу, включая вершины области. <br/>
        /// Перед использованием, необходимо вызвать <see cref="BoundaryBase.Initialize(GeneratorBase)"/>
        /// </summary>
        public IHPoint[] Points
        {
            get
            {
                if (this.points is null)
                    throw new ArgumentNullException(
                        $"перед обращением к {nameof(Points)} требуется вызвать {nameof(Initialize)}.");
                return points;
            }
        }
        
        protected IHPoint[] vertexes;
        /// <summary>
        /// Вершины, задающие форму оболочки границы
        /// </summary>
        public IHPoint[] Vertexes => vertexes;

        /// <summary>
        /// Общее количество граничных точек
        /// </summary>
        public int Length => points.Length;

        protected int[] vertexesIds = null;
        /// <summary>
        /// Индексы вершин, образующих область.
        /// Первая вершина идет с нулевым индексом
        /// </summary>
        public int[] VertexesIds => vertexesIds;

        /// <summary>
        /// Инициализация границы, генерация точек между вершинами, образующими оболочку границы
        /// </summary>
        /// <param name="generator">Способ генерации точек на границе</param>
        /// <returns>Сгенерированное множество точек границы, в т.ч. вершины, образующие область границы</returns>
        public IHPoint[] Initialize(GeneratorBase generator)
        {
            //генерируем множество граничных узлов
            this.points = generator.Generate(this);
            
            //сохраняем индексы вершин, образующих область
            vertexesIds = new int[this.Vertexes.Length];
            int currentVertexId = 0;
            for (int i = 0; i < points.Length; i++)
            {
                //if (Vertexes[currentVertexId] == points[i])
                if (Vertexes[currentVertexId].X == points[i].X && Vertexes[currentVertexId].Y == points[i].Y)
                {
                    vertexesIds[currentVertexId] = i;
                    currentVertexId++;
                    if (currentVertexId == vertexesIds.Length)
                        break;
                }
            }

            InitializeSquare();

            return this.points;
        }


        protected IHPoint[] outRect = null;
        /// <summary>
        /// Прямоугольник, описанный около текущей ограниченной области
        /// </summary>
        public IHPoint[] OutRect => outRect;

        /// <summary>
        /// Вычислить 4 вершины, которые образуют прямоугольник,
        /// в который можно вписать текущую ограниченную область
        /// </summary>
        protected void InitializeSquare()
        {
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            //собираем края области
            foreach (var vertex in this.Vertexes)
            {
                if (vertex.X < minX)
                    minX = vertex.X;
                if (vertex.X > maxX)
                    maxX = vertex.X;
                if (vertex.Y < minY)
                    minY = vertex.Y;
                if (vertex.Y > maxY)
                    maxY = vertex.Y;
            }

            //формируем описанный прямоугольник
            IHPoint[] rectangle = new IHPoint[4];
            rectangle[0] = new HPoint(minX, minY);
            rectangle[1] = new HPoint(minX, maxY);
            rectangle[2] = new HPoint(maxX, maxY);
            rectangle[3] = new HPoint(maxX, minY);
            this.outRect = rectangle;
        }


        /// <summary>
        /// Объявить конкретную область границы. <br/>
        /// В дальнейшем потребуется вызвать <see cref="BoundaryBase.Initialize(GeneratorBase)"/> для генерации границы
        /// </summary>
        /// <param name="vertexes">множество точек, формирующее область объявляемой границы</param>
        /// <param name="basePoints">базовое множество точек, которое будет ограничено текущей границей</param>
        public BoundaryBase(IHPoint[] vertexes)
        {
            this.vertexes = vertexes;
        }

        /// <summary>
        /// Перед использованием, необходимо вызвать <see cref="BoundaryBase.Initialize(GeneratorBase)"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IHPoint this[int index]
        {
            get
            {
                if (this.points is null)
                    throw new ArgumentNullException(
                        $"перед обращением к {nameof(Points)} требуется вызвать {nameof(Initialize)}.");
                return this.points[index];
            }
        }
    }
}
