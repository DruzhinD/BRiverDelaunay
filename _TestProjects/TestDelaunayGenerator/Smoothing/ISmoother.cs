using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace TestDelaunayGenerator.Smoothing
{
    public interface ISmoother
    {
        /// <summary>
        /// Сглаживание сетки. Использует TreeMash, модифицирует существующую сетку
        /// </summary>
        void Smooth(IMesh baseMash);
    }
}
