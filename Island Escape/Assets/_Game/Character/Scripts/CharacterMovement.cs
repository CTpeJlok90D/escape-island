using TNRD;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Custom;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    [SerializeField] private SerializableInterface<IContainsCharacter> _characterContainer;
    [SerializeField] private NetworkRigidbody _characterRigidbody;

    private float _minXAngle = 10;
    private float _maxXAngle = 170;
    private float _acceseleration = 20;
    
    public NetVariable<Vector2> MoveDirection { get; private set; }
    public NetVariable<Vector3> Rotation { get; private set; }

    public float MoveSpeed => _characterContainer.Value.Data.Value.BaseMoveSpeed;

    private void Awake()
    {
        MoveDirection = new NetVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        Rotation = new NetVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }

    private void Update()
    {
        if (IsOwner == false)
        {
            return;
        }
        
        Vector3 moveResult = MoveDirection.Value;
        moveResult = new Vector3(moveResult.x, 0, moveResult.y);
        moveResult = _characterRigidbody.transform.TransformDirection(moveResult);
        
        Vector3 rotation = new Vector3(Rotation.Value.x, Rotation.Value.y, _characterRigidbody.transform.eulerAngles.z);
        rotation.x = Mathf.Clamp(rotation.x, _minXAngle, _maxXAngle);
        Rotation.Value = rotation;
        
        Vector3 currentVelocity = _characterRigidbody.GetLinearVelocity();
        Vector3 velocity = Vector3.MoveTowards(currentVelocity, moveResult * MoveSpeed * 2.5f, Time.deltaTime * _acceseleration);
        
        _characterRigidbody.SetLinearVelocity(velocity);
        _characterRigidbody.transform.eulerAngles = new Vector3(0, rotation.y, 0);
    }
}
