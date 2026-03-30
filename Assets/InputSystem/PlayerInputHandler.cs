using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Player _player;
    private FireShooting _fireShooting;
    private IceShooting _iceShooting;
    private PlayerControls _controls;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _fireShooting = GetComponent<FireShooting>();
        _iceShooting = GetComponent<IceShooting>();
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();

        _controls.Player.Jump.performed += OnJump;
        _controls.Player.FireShoot.performed += OnFireShoot;
        _controls.Player.IceShoot.performed += OnIceShoot;
    }

    private void OnDisable()
    {
        _controls.Player.Jump.performed -= OnJump;
        _controls.Player.FireShoot.performed -= OnFireShoot;
        _controls.Player.IceShoot.performed -= OnIceShoot;
        _controls.Disable();
    }

    public void DisablePlayerControls()
    {
        _controls.Player.Disable();
    }

    public void EnablePlayerControls()
    {
        _controls.Player.Enable();
    }

    private void OnJump(InputAction.CallbackContext context) => _player.Jump();
    private void OnFireShoot(InputAction.CallbackContext context) => _fireShooting.Shoot();
    private void OnIceShoot(InputAction.CallbackContext context) => _iceShooting.Shoot();
}
