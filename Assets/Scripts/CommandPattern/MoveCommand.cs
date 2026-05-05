using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform _transform;
    private Vector3 _prevPos;
    private Quaternion _prevRot;
    private Vector3 _newPos;
    private Quaternion _newRot;

    public MoveCommand(Transform transform, Vector3 newPos, Quaternion newRot)
    {
        _transform = transform;
        // Используем local, если объект привязан к камере
        _prevPos = transform.localPosition;
        _prevRot = transform.localRotation;
        _newPos = newPos;
        _newRot = newRot;
    }

    public void Execute() => SetState(_newPos, _newRot);
    public void Undo() => SetState(_prevPos, _prevRot);

    private void SetState(Vector3 pos, Quaternion rot)
    {
        _transform.localPosition = pos;
        _transform.localRotation = rot;
    }
}
