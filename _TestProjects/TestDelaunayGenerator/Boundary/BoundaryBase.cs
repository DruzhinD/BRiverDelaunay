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
        /// <summary>
        /// базовое множество точек, которое текущая область будет ограничивать
        /// </summary>
        protected readonly IHPoint[] basePoints;


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
        
        /// <summary>
        /// Инициализация границы, генерация точек между вершинами, образующими оболочку границы
        /// </summary>
        /// <param name="generator">Способ генерации точек на границе</param>
        /// <returns>Сгенерированное множество точек границы, в т.ч. вершины, образующие область границы</returns>
        public IHPoint[] Initialize(GeneratorBase generator)
        {
            this.points = generator.Generate(this);
            return this.points;
        }


        /// <summary>
        /// Объявить конкретную область границы. <br/>
        /// В дальнейшем потребуется вызвать <see cref="BoundaryBase.Initialize(GeneratorBase)"/> для генерации границы
        /// </summary>
        /// <param name="vertexes">множество точек, формирующее область объявляемой границы</param>
        /// <param name="basePoints">базовое множество точек, которое будет ограничено текущей границей</param>
        public BoundaryBase(IHPoint[] vertexes, IHPoint[] basePoints)
        {
            this.vertexes = vertexes;
            this.basePoints = basePoints;
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
