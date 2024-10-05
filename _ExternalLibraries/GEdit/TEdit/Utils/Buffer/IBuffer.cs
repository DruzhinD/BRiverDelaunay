namespace TEdit.Utils.Buffer
{
    public enum BufferTarget : byte
    {
        ColorBuffer,
        IndexBuffer,
        VertexBuffer
    }
    /// <summary>
    /// Буфер данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBuffer<T> where T : struct
    {
        /// <summary>
        /// Получить содержимое буфера.
        /// </summary>
        T[] Data { get; }
        /// <summary>
        /// Получить  размер буфера.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Получает размер одного элемента в буфере (т.е. 2 для 2D-точек или 3 для треугольников).
        /// </summary>
        int Size { get; }
        /// <summary>
        /// Получить тип буфера (вершины или индексы).
        /// </summary>
        BufferTarget Target { get; }
    }
}
