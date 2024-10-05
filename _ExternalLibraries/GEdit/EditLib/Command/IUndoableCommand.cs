namespace EditLib
{
    /// <summary>
    /// Определение команды, обладающей возможностью отмены
    /// </summary>
    public interface ICommand
    {
        string Name { get; set; }
        void Undo();
        void Redo();
    }
}