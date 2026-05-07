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
    [SerializeField] private int _maxStackSize = 1000;

    public static bool IsReversing { get; private set; }
    private CommandManager _commandManager;
   
    private Vector3 _lastCameraPos;

    void Awake()
    {
        GameEvents.OnTimeReverseActivated += StartReverse;
        _commandManager = new CommandManager(_maxStackSize);
    }

    void FixedUpdate()
    {
        if (!IsReversing)
        {
            _commandManager.ExecuteCommand(new MoveCommand(_playerTransform, _playerTransform.localPosition, _playerTransform.localRotation));
            _commandManager.ExecuteCommand(new HealthCommand(_healthSystem, _healthSystem.CurrentHealth));

            if (!_cameraMoving.IsTransitioning)
            {
                _commandManager.ExecuteCommand(new MoveCommand(_cameraTransform, _cameraTransform.position, _cameraTransform.rotation));
            }
        }
    }

    public void StartReverse()
    {
        if (IsReversing) return;
        IsReversing = true;

        //_cameraMoving.SetActive(false);
        _inputHandler.DisablePlayerControls();

        // Замораживаем физику, чтобы гравитация не копилась
        _playerRb.simulated = false;

        StartCoroutine(SmoothUndo());
    }

    public void StopReverse()
    {
        if (!IsReversing) return;
        IsReversing = false;

        // Включаем физику обратно
        _playerRb.simulated = true;
        // ОБНУЛЯЕМ скорость, чтобы не было резкого рывка вниз
        _playerRb.velocity = Vector2.zero;

        _inputHandler.EnablePlayerControls();

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.TakeItSlow(1.5f);
        }
    }

    private IEnumerator SmoothUndo()
    {
        yield return null;

        while (IsReversing && _commandManager.HasCommands())
        {
            int commandsToUndo = 3 * _reverseSpeed;

            for (int i = 0; i < commandsToUndo; i++)
            {
                if (_commandManager.HasCommands())
                {
                    _commandManager.UndoLastCommand();
                }
            }
            yield return new WaitForFixedUpdate();
        }
        StopReverse();
    }

    public void ResetHistory()
    {
        _commandManager.ClearHistory();
        _lastCameraPos = _cameraTransform.position;
    }

    private void OnDestroy()
    {
        GameEvents.OnTimeReverseActivated -= StartReverse;
    }
}
