using TNRD;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour, IHaveVelocity
{
    [SerializeField] private SerializableInterface<IContainsCharacter> _characterContainer;
    [SerializeField] private CharacterController _characterController;

    private float _minXAngle = 10;
    private float _maxXAngle = 170;
    private float _acceseleration = 20;
    
    public NetVariable<Vector2> MoveDirectionNet { get; private set; }
    public NetVariable<Vector3> RotationNet { get; private set; }
    public NetVariable<Vector3> VelocityNet { get; private set; }

    public Vector3 Velocity => VelocityNet.Value;
    
    public float MoveSpeed => _characterContainer.Value.Data.Value.BaseMoveSpeed;

    private void Awake()
    {
        VelocityNet = new NetVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        MoveDirectionNet = new NetVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        RotationNet = new NetVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
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
        
        _characterController.Move(VelocityNet.Value);
        _characterController.transform.eulerAngles = new Vector3(0, rotation.y, 0);
    }
}
