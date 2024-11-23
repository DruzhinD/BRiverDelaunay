using CommonLib;

namespace MeshLib.Smoothing
{
    public interface ISmoother
    {
        /// <summary>
        /// Сглаживание сетки. Использует TreeMash, модифицирует существующую сетку
        /// </summary>
        void Smooth(IMesh baseMash);
    }
}
