using System.Drawing;

namespace EditLib
{
    public interface IGeometry
    {
        bool IsClosed { get; }
    }

    public interface ITransformedGeometry : IGeometry
    {
        byte[] GetTransformedPointTypes(Figure owner);
        PointF[] GetTransformedPoints(Figure owner);
        void SetTransformedPoints(Figure owner, PointF[] points);
    }
}
