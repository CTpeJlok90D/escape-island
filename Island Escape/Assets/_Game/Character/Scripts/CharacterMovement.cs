using System;
using System.Collections;
using TNRD;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour, IHaveVelocity, ICanBePlacedOnGround
{
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private SerializableInterface<IContainsCharacter> _characterContainer;
    [SerializeField] private CharacterController _characterController;
    
    private float _minXAngle = 10;
    private float _maxXAngle = 170;
    private float _sprintMovementModificator = 1.75f;
    private float _jumpStrengthMultiplier = 0.05f;

    private Coroutine _jumpCoroutine;
    private float _jumpCooldown = 0.5f;
    
    public NetVariable<Vector2> MoveDirectionNet { get; private set; }
    public NetVariable<Vector3> RotationNet { get; private set; }
    public NetVariable<Vector3> VelocityNet { get; private set; }
    public NetVariable<bool> IsSprinting { get; private set; }

    public Vector3 Velocity => VelocityNet.Value;

    public float MoveSpeed => _characterContainer.Value.Data.Value.BaseMoveSpeed;

    public bool IsGrounded
    {
        get
        {
            Vector3 raycastOrigin = _characterController.transform.position + new Vector3(0, _characterController.radius, 0);
            Physics.SphereCast(raycastOrigin, _characterController.radius, Vector3.down, out RaycastHit hitInfo, 0.2f);
            
            return hitInfo.collider != null;
        }
    }

    public event Action Jumped;

    private void Awake()
    {
        VelocityNet = new NetVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        MoveDirectionNet = new NetVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        RotationNet = new NetVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        IsSprinting = new NetVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }

    private void Update()
    {
        if (IsOwner == false)
        {
            return;
        }
        
        Vector3 moveResult = MoveDirectionNet.Value;
        moveResult = new Vector3(moveResult.x, 0, moveResult.y);
        moveResult = _characterController.transform.TransformDirection(moveResult);
        
        Vector3 rotation = new Vector3(RotationNet.Value.x, RotationNet.Value.y, _characterController.transform.eulerAngles.z);
        rotation.x = Mathf.Clamp(rotation.x, _minXAngle, _maxXAngle);
        
        RotationNet.Value = rotation;
        VelocityNet.Value = moveResult * MoveSpeed * Time.deltaTime + Physics.gravity * Time.deltaTime;
        if (IsSprinting.Value)
        {
            VelocityNet.Value *= _sprintMovementModificator;
        }
        
        _characterController.Move(VelocityNet.Value);
        _characterController.transform.eulerAngles = new Vector3(0, rotation.y, 0);
    }

    public void Jump()
    {
        if (IsGrounded == false || _jumpCoroutine != null)
        {
            return;
        }
        
        _jumpCoroutine = StartCoroutine(JumpCoroutine());
        Jumped?.Invoke();
    }
    
    private IEnumerator JumpCoroutine()
    {
        for (float i = 0; i < _jumpCurve.keys[^1].time; i += Time.deltaTime)
        {
            Vector3 moveDirection = new Vector3(0, _jumpCurve.Evaluate(i) * _jumpStrengthMultiplier, 0);
            _characterController.Move(moveDirection);
            yield return null;
        }

        yield return new WaitForSeconds(_jumpCooldown);
        _jumpCoroutine = null;
    }
}
