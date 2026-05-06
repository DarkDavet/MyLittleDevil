using System.Collections;
using System.Collections.Generic;

public class CommandManager 
{
    private LimitedStack<ICommand> _commandStack;
    private int _maxStackSize;

    public CommandManager(int maxStackSize)
    {
        _maxStackSize = maxStackSize;
        _commandStack = new LimitedStack<ICommand>(maxStackSize);
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandStack.Push(command);
    }

    public void UndoLastCommand()
    {
        if (_commandStack.Count > 0)
        {
            ICommand command = _commandStack.Pop();
            command.Undo();
        }
    }


    public void ClearHistory()
    {
        _commandStack = new LimitedStack<ICommand>(_maxStackSize);
    }

    public bool HasCommands()
    {
        return _commandStack.Count > 0;
    }
}
