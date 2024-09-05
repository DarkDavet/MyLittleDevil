using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform _transform;
    private Vector3 _position;
    private Vector3 _previousPosition;

    public MoveCommand(Transform transform, Vector3 position)
    {
        _transform = transform;
        _position = position;
        _previousPosition = transform.position;
    }
    public void Execute()
    {
        _transform.position = _position;
    }

    public void Undo()
    {
        _transform.position = _previousPosition;
    }
}
