using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPlayerControl : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _rotateAction;

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