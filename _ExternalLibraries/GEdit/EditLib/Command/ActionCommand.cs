using System;

namespace EditLib
{
    /// <summary>
    /// Определение команды-действия
    /// </summary>
    public class ActionCommand : ICommand
    {
        public string Name { get; set; }

        readonly Action _redo;
        readonly Action _undo;

        /// <summary>
        /// Запоминаем действий отмены и возврата при создании команды-действия
        /// </summary>
        /// <param name="undo"></param>
        /// <param name="redo"></param>
        public ActionCommand(Action undo, Action redo)
        {
            _redo = redo;
            _undo = undo;
        }

        /// <summary>
        /// Выполняем запомненное в конструкторе действие по отмене
        /// </summary>
        public void Undo()
        {
            if (_undo != null)
                _undo();
        }

        /// <summary>
        /// Выполняем запомненное в конструкторе действие по возврату
        /// </summary>
        public void Redo()
        {
            if (_redo != null)
                _redo();
        }
    }
}