namespace TEdit.Utils.Buffer
{

    public class IndexBuffer : BufferBase<int>
    {
        public IndexBuffer(int capacity, int size)
            : base(capacity, size)
        {
        }
        public IndexBuffer(int[] data, int size)
            : base(data, size)
        {
        }
        public override int Size => size;
        public override BufferTarget Target => BufferTarget.IndexBuffer;
    }
}


