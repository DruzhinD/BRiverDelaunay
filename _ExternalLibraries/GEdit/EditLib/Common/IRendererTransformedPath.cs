using System.Drawing;
using System.Drawing.Drawing2D;

namespace EditLib
{
    public interface IRendererTransformedPath
    {
        GraphicsPath GetTransformedPath(Graphics graphics, Figure figure);
    }
}
