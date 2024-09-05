using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReverseController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _camera;
    private CommandManager _commandManager;
    public static bool isReversing = false;
    private Rigidbody2D rbPlayer;
    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = _player.GetComponent<Rigidbody2D>();
        _commandManager = new CommandManager(20);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _player.CheckBirdStatus())
        {
            isReversing = !isReversing;
            if (isReversing)
            {
                StartCoroutine(SmoothUndoAllCommands());
            }
            isReversing = !isReversing;
        }
        if (!isReversing)
        {
            if (Input.GetButtonDown("Jump") && _player.CheckBirdStatus())
            {
                Vector3 newPosition = rbPlayer.transform.position;       
                ICommand moveCommand = new MoveCommand(_player.transform, newPosition);
                _commandManager.ExecuteCommand(moveCommand);

                Vector3 newPositionCam = _camera.position;
                ICommand moveCommandCam = new MoveCommand(_camera, newPositionCam);
                _commandManager.ExecuteCommand(moveCommandCam);
            }
        }
    }

    private IEnumerator SmoothUndoAllCommands()
    {
        while (_commandManager.HasCommands())
        {
            _commandManager.UndoLastCommand();
            yield return new WaitForSeconds(0.01f); // Adjust the delay for smoother transition
        }
    }
}
