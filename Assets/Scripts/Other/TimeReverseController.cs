using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReverseController : MonoBehaviour
{
    [SerializeField] private CameraMoving _cameraMoving;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private int _reverseSpeed = 3;

    private CommandManager _commandManager;
    private bool _isReversing = false;

    void Awake() => _commandManager = new CommandManager(500); // Для камеры и игрока нужно побольше места

    void FixedUpdate()
    {
        if (!_isReversing)
        {
            // Записываем локальную позицию игрока (относительно камеры)
            _commandManager.ExecuteCommand(new MoveCommand(_playerTransform, _playerTransform.localPosition, _playerTransform.localRotation));

            // Записываем мировую позицию камеры
            _commandManager.ExecuteCommand(new MoveCommand(_cameraTransform, _cameraTransform.position, _cameraTransform.rotation));
        }
    }

    public void StartReverse()
    {
        _isReversing = true;
        _cameraMoving.SetActive(false); // Твой метод из CameraMoving (плавная остановка)
        StartCoroutine(SmoothUndo());
    }

    public void StopReverse()
    {
        _isReversing = false;
        _cameraMoving.SetActive(true); // Запуск камеры после перемотки
    }

    private IEnumerator SmoothUndo()
    {
        while (_isReversing && _commandManager.HasCommands())
        {
            // Цикл для ускорения: выполняем Undo несколько раз за один yield
            for (int i = 0; i < _reverseSpeed; i++)
            {
                if (_commandManager.HasCommands())
                {
                    _commandManager.UndoLastCommand(); 
                    _commandManager.UndoLastCommand(); 
                }
            }

            yield return new WaitForFixedUpdate();
        }
        StopReverse();
    }
}
