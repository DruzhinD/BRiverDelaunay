namespace TEdit.Utils.Buffer
{
    public class VertexBuffer : BufferBase<float>
    {
        public VertexBuffer(int capacity, int size = 2)
            : base(capacity, size)
        {
        }

        public VertexBuffer(float[] data, int size = 2)
            : base(data, size)
        {
        }
        public override int Size => size;
        public override BufferTarget Target => BufferTarget.VertexBuffer;
    }
}
