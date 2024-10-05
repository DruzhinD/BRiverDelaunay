using System.Drawing;

namespace EditLib
{
    public interface IGradientFill
    {
        PointF[] GetGradientPoints(Figure figure);
        void SetGradientPoints(Figure figure, PointF[] points);
        Color GradientColor { get; set; }
    }
}
