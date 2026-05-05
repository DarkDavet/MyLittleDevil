using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReverseController : MonoBehaviour
{
    [SerializeField] private CameraMoving _cameraMoving;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerHealthSystem _healthSystem;
    [SerializeField] private int _reverseSpeed = 3;

    private CommandManager _commandManager;
    private bool _isReversing = false;

    void Awake() => _commandManager = new CommandManager(500); 

    void FixedUpdate()
    {
        if (!_isReversing)
        {
            // Записываем локальную позицию игрока (относительно камеры)
            _commandManager.ExecuteCommand(new MoveCommand(_playerTransform, _playerTransform.localPosition, _playerTransform.localRotation));

            // Записываем мировую позицию камеры
            _commandManager.ExecuteCommand(new MoveCommand(_cameraTransform, _cameraTransform.position, _cameraTransform.rotation));

            _commandManager.ExecuteCommand(new HealthCommand(_healthSystem, _healthSystem.CurrentHealth));
        }
    }

    public void StartReverse()
    {
        if (_isReversing) return;
        _isReversing = true;

        _cameraMoving.SetActive(false);
        _inputHandler.DisablePlayerControls();

        // Замораживаем физику, чтобы гравитация не копилась
        _playerRb.simulated = false;

        StartCoroutine(SmoothUndo());
    }

    public void StopReverse()
    {
        if (!_isReversing) return;
        _isReversing = false;

        // Включаем физику обратно
        _playerRb.simulated = true;
        // ОБНУЛЯЕМ скорость, чтобы не было резкого рывка вниз
        _playerRb.velocity = Vector2.zero;

        _cameraMoving.SetActive(true);
        _inputHandler.EnablePlayerControls();

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.TakeItSlow();
        }
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
                    _commandManager.UndoLastCommand(); 
                }
            }

            yield return new WaitForFixedUpdate();
        }
        StopReverse();
    }
}
