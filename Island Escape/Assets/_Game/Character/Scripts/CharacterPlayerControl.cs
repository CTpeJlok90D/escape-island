using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPlayerControl : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _rotateAction;
    [SerializeField] private InputActionReference _sprintAction;
    [SerializeField] private InputActionReference _jumpAction;

    private void OnEnable()
    {
        _sprintAction.action.started += OnSprintStart;
        _sprintAction.action.canceled += OnSprintCancel;
        _jumpAction.action.started += OnJumpStart;
    }

    private void OnDisable()
    {
        _sprintAction.action.started -= OnSprintStart;
        _sprintAction.action.canceled -= OnSprintCancel;
        _jumpAction.action.started -= OnJumpStart;
    }

    private void OnJumpStart(InputAction.CallbackContext obj)
    {
        _characterMovement.Jump();
    }

    private void OnSprintStart(InputAction.CallbackContext obj)
    {
        _characterMovement.IsSprinting.Value = true;
    }
    
    private void OnSprintCancel(InputAction.CallbackContext obj)
    {
        _characterMovement.IsSprinting.Value = false;
    }

    private void Update()
    {
        if (_characterMovement.IsOwner == false)
        {
            return;
        }
        
        Vector2 moveInput = _moveAction.action.ReadValue<Vector2>();
        Vector2 rotationInput = _rotateAction.action.ReadValue<Vector2>();
        
        rotationInput = new Vector2(-rotationInput.y, rotationInput.x);
        
        _characterMovement.MoveDirectionNet.Value = new Vector2(moveInput.x, moveInput.y);
        _characterMovement.RotationNet.Value += (Vector3)rotationInput;
    }
}