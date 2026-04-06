using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InventoryObject _inventory;
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

        if (_player != null) _controls.Player.Jump.performed += OnJump;
        if (_fireShooting != null) _controls.Player.FireShoot.performed += OnFireShoot;
        if (_iceShooting != null) _controls.Player.IceShoot.performed += OnIceShoot;

        _controls.Player.UseSlot1.performed += ctx => TryUseSlot(0);
        _controls.Player.UseSlot2.performed += ctx => TryUseSlot(1);
        _controls.Player.UseSlot3.performed += ctx => TryUseSlot(2);
    }

    private void OnDisable()
    {
        if (_player != null) _controls.Player.Jump.performed -= OnJump;
        if (_fireShooting != null) _controls.Player.FireShoot.performed -= OnFireShoot;
        if (_iceShooting != null) _controls.Player.IceShoot.performed -= OnIceShoot;
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

    private void TryUseSlot(int slotIndex)
    {
        if (slotIndex < _inventory.Count)
        {
            ItemObject itemToUse = _inventory[slotIndex].item;

            if (itemToUse != null)
            {
                _inventory.OnItemClick(itemToUse);
                Debug.Log($"??????? ???????: ??????????? {itemToUse.name} ?? ????? {slotIndex + 1}");
            }
        }
        else
        {
            Debug.Log($"???? {slotIndex + 1} ????");
        }
    }

    private void OnJump(InputAction.CallbackContext context) => _player.Jump();
    private void OnFireShoot(InputAction.CallbackContext context) => _fireShooting?.Shoot();
    private void OnIceShoot(InputAction.CallbackContext context) => _iceShooting?.Shoot();
}
