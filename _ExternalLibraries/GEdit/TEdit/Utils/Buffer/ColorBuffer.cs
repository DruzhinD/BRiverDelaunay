namespace TEdit.Utils.Buffer
{
    using System.Drawing;

    public class ColorBuffer : BufferBase<Color>
    {
        public ColorBuffer(int capacity, int size = 1)
            : base(capacity, 1)
        {
        }
        public ColorBuffer(Color[] data, int size = 1)
            : base(data, 1)
        {
        }
        public override int Size => size;
        public override BufferTarget Target => BufferTarget.ColorBuffer;
    }
}


