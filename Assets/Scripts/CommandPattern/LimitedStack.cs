using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedStack<T>
{
    private readonly T[] _items;
    private int _start;
    private int _end;
    private int _count;
    private readonly int _capacity;

    public LimitedStack(int capacity)
    {
        _capacity = capacity;
        _items = new T[capacity];
    }

    public void Push(T item)
    {
        _items[_end] = item;
        _end = (_end + 1) % _capacity;
        if (_count < _capacity) _count++;
        else _start = (_start + 1) % _capacity; 
    }

    public T Pop()
    {
        if (_count == 0) return default;
        _end = (_end - 1 + _capacity) % _capacity;
        T item = _items[_end];
        _count--;
        return item;
    }

    public int Count => _count;
}
