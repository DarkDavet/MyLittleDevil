using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    private LimitedStack<ICommand> _commandStack;

    public CommandManager(int maxStackSize)
    {
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

    public void UndoAllCommands()
    {
        while (_commandStack.Count > 0)
        {
            UndoLastCommand();
        }
    }

    public bool HasCommands()
    {
        return _commandStack.Count > 0;
    }
}
