using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedStack<T>
{
    private readonly Stack<T> _stack = new Stack<T>();
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly int _maxSize;

    public LimitedStack(int maxSize)
    {
        _maxSize = maxSize;
    }

    public void Push(T item)
    {
        if (_stack.Count >= _maxSize)
        {
            List<T> tempList = new List<T>(_stack);
            tempList.Reverse();
            tempList.RemoveAt(0);

            _stack.Clear();
            foreach (var element in tempList)
            {
                _stack.Push(element);
            }
        }
        _stack.Push(item);
    }

    public T Pop()
    {
        return _stack.Pop();
    }

    public T Peek()
    {
        return _stack.Peek();
    }

    public int Count => _stack.Count;

    public bool Any() => _stack.Count > 0;
}
