using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeReverseController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    
    [SerializeField] private int _reverseSpeed = 3;
    [SerializeField] private int _maxStackSize = 1000;

    private Transform _playerTransform;
    private Rigidbody2D _playerRb;
    private PlayerInputHandler _inputHandler;
    private PlayerHealthSystem _healthSystem;

    private CameraMoving _cameraMoving;
    private Transform _cameraTransform;

    public static bool IsReversing { get; private set; }
    private CommandManager _commandManager;
   
    private Vector3 _lastCameraPos;

    void Awake()
    {
        GameEvents.OnTimeReverseActivated += StartReverse;
        _commandManager = new CommandManager(_maxStackSize);

        _playerTransform = _player.GetComponent<Transform>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _inputHandler = _player.GetComponent<PlayerInputHandler>();
        _healthSystem = _player.GetComponent<PlayerHealthSystem>();

        _cameraMoving = _camera.GetComponent<CameraMoving>();
        _cameraTransform = _camera.GetComponent<Transform>();
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
            // Если игра на паузе — просто ждем и ничего не делаем
            if (Time.timeScale <= 0)
            {
                yield return null;
                continue;
            }

            int commandsToUndo = 3 * _reverseSpeed;

            for (int i = 0; i < commandsToUndo; i++)
            {
                if (_commandManager.HasCommands())
                {
                    _commandManager.UndoLastCommand();
                }
            }

            // Вместо WaitForFixedUpdate используем это, чтобы корутина не застревала в паузе
            yield return new WaitForEndOfFrame();
        }

        if (Time.timeScale > 0) StopReverse();
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
