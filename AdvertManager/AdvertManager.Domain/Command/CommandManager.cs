using System.Collections.Generic;

namespace AdvertManager.Domain.Command
{
    public class CommandManager
    {
        private readonly Stack<IAdvertisementCommand> undoStack = new Stack<IAdvertisementCommand>();
        private readonly Stack<IAdvertisementCommand> redoStack = new Stack<IAdvertisementCommand>();

        public void ExecuteCommand(IAdvertisementCommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var cmd = undoStack.Pop();
                cmd.Unexecute();
                redoStack.Push(cmd);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var cmd = redoStack.Pop();
                cmd.Execute();
                undoStack.Push(cmd);
            }
        }
        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public IAdvertisementCommand PeekUndo() => undoStack.Peek();
        public IAdvertisementCommand PeekRedo() => redoStack.Peek();
    }
}
