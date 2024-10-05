﻿// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;

    /// <summary>
    /// Configure advanced aspects of the library.
    /// Настроить расширенные аспекты библиотеки.
    /// </summary>
    public class Configuration
    {
        public Configuration()
            : this(() => RobustPredicates.Default, () => new TrianglePool())
        {
        }

        public Configuration(Func<IPredicates> predicates)
            : this(predicates, () => new TrianglePool())
        {
        }

        public Configuration(Func<IPredicates> predicates, Func<TrianglePool> trianglePool)
        {
            Predicates = predicates;
            TrianglePool = trianglePool;
        }

        /// <summary>
        /// Gets or sets the factory method for the <see cref="IPredicates"/> implementation.
        /// Получает или задает фабричный метод для реализации <see cref = "IPredicates" />.
        /// </summary>
        public Func<IPredicates> Predicates { get; set; }

        /// <summary>
        /// Gets or sets the factory method for the <see cref="TrianglePool"/>.
        /// Получает или задает фабричный метод для <см. Cref = "TrianglePool" />.
        /// </summary>
        public Func<TrianglePool> TrianglePool { get; set; }
    }
}
