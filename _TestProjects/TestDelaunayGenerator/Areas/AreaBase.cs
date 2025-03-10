using CommonLib.Geometry;
using TestDelaunayGenerator.Boundary;

namespace TestDelaunayGenerator.Areas
{
    /// <summary>
    /// Базовый абстрактный метод генерации множества точек/узлов области для будущей триангуляции
    /// </summary>
    public abstract class AreaBase
    {
        /// <summary>
        /// Название полигона, области
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Узлы/точки триангуляции
        /// </summary>
        protected IHPoint[] points;

        /// <summary>
        /// Узлы/точки триангуляции
        /// </summary>
        public IHPoint[] Points => points;

        /// <summary>
        /// контейнер, содержащий в себе множество ограниченных областей
        /// </summary>
        protected BoundaryContainer boundaryContainer = null;

        /// <summary>
        /// контейнер, содержащий в себе множество ограниченных областей
        /// </summary>
        public BoundaryContainer BoundaryContainer => boundaryContainer;

        /// <summary>
        /// Генератор границы
        /// </summary>
        public GeneratorBase BoundaryGenerator { get; set; } = new GeneratorFixed(0);

        /// <summary>
        /// Количество точек триангуляции
        /// </summary>
        protected int Count = 100;


        /// <summary>
        /// Базовый абстрактный метод генерации множества точек/узлов области для будущей триангуляции
        /// </summary>
        protected AreaBase(int count = 10_000)
        {
            this.Count = count;

            //this.Initialize();
        }

        /// <summary>
        /// Инициализация области, вызывается в конструкторе
        /// </summary>
        public abstract void Initialize();

        
        public void AddBoundary(IHPoint[] boundary)
        {
            if (boundaryContainer is null)
                boundaryContainer = new BoundaryContainer(BoundaryGenerator);
            boundaryContainer.Add(boundary);
        }
        
    }
}
