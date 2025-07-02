using UnityEngine;

public class CharacterMovementLinker : MonoBehaviour
{
    [SerializeField] private CharacterViewInstance _characterInstance;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _zMovementParametrName = "Z Movement";
    [SerializeField] private string _xMovementParametrName = "X Movement";
    [SerializeField] private string _moveSpeedParametrName = "Move speed";
    private readonly float _velocitySensitivity = 170;

    private IHaveVelocity _velocity;
    
    private void Start()
    {
        CharacterInstanceReference characterPawn = _characterInstance.LinkedCharacter as CharacterInstanceReference;
        
        if (characterPawn == null)
        {
            Debug.LogError("CharacterInstanceReference is null");
            enabled = false;
            return;
        }
        
        _velocity = characterPawn.GetComponent<IHaveVelocity>();
        if (_velocity == null)
        {
            Debug.LogWarning($"{nameof(IHaveVelocity)} component was not found");
        }
    }

    private void LateUpdate()
    {
        if (_velocity == null)
        {
            return;
        }
        
        Vector3 velocity = _velocity.Velocity * _velocitySensitivity;
        Vector3 localVelocity = _characterInstance.transform.InverseTransformDirection(velocity);
        
        Vector3 animationVelocity = new(_animator.GetFloat(_xMovementParametrName), 0, _animator.GetFloat(_zMovementParametrName));
        Vector3 smoothedVelocity = Vector3.MoveTowards(animationVelocity, localVelocity, Time.deltaTime * 20);

        _animator.SetFloat(_zMovementParametrName, smoothedVelocity.z);
        _animator.SetFloat(_xMovementParametrName, smoothedVelocity.x);
        _animator.SetFloat(_moveSpeedParametrName, smoothedVelocity.magnitude);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_characterInstance == null)
        {
            _characterInstance = GetComponent<CharacterViewInstance>();
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
#endif
}