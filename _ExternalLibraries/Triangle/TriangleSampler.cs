// -----------------------------------------------------------------------
// <copyright file="TriangleSampler.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.Cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------
// <// <файл авторских прав="TriangleSampler.cs">
// Оригинальный код треугольника Джонатана Ричарда Шевчука, http://www.cs.Cmu.edu /~quake/triangle.html
// Треугольник.СЕТЕВОЙ код Кристиана Вольтеринга, http://triangle.codeplex.com/
// </авторское право>
// -----------------------------------------------------------------------
namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Topology;

    /// <summary>
    /// Используется для выборки треугольников в классе TriangleLocator.
    /// </summary>
    class TriangleSampler : IEnumerable<Triangle>
    {
        private const int RANDOM_SEED = 110503;

        // Фактор, подобранный эмпирически.
        private const int samplefactor = 11;

        private Random random;
        private IMeshNet mesh;

        // Количество случайных выборок для местоположения точки (не менее 1).
        private int samples = 1;

        // Number of triangles in mesh.
        private int triangleCount = 0;

        public TriangleSampler(IMeshNet mesh)
            : this(mesh, new Random(RANDOM_SEED))
        {
        }

        public TriangleSampler(IMeshNet mesh, Random random)
        {
            this.mesh = mesh;
            this.random = random;
        }

        /// <summary>
        /// Сбросьте пробоотборник.
        /// </summary>
        public void Reset()
        {
            this.samples = 1;
            this.triangleCount = 0;
        }

        /// <summary>
        /// Обновите параметры выборки, если сетка изменилась.
        /// </summary>
        public void Update()
        {
            int count = mesh.triangles.Count;

            if (triangleCount != count)
            {
                triangleCount = count;

                // The number of random samples taken is proportional to the cube root
                // of the number of triangles in the mesh.  The next bit of code assumes
                // that the number of triangles increases monotonically (or at least
                // doesn't decrease enough to matter).

                // Количество взятых случайных выборок пропорционально кубическому корню
                // количества треугольников в сетке. Следующий фрагмент кода предполагает
                // что количество треугольников монотонно увеличивается (или хотя бы
                // не уменьшается настолько, чтобы иметь значение).
                while (samplefactor * samples * samples * samples < count)
                {
                    samples++;
                }
            }
        }

        public IEnumerator<Triangle> GetEnumerator()
        {
            return mesh.triangles.Sample(samples, random).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
