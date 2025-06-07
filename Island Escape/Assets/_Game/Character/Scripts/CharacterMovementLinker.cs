using UnityEngine;

public class CharacterMovementLinker : MonoBehaviour
{
    [SerializeField] private CharacterViewInstance _characterController;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _zMovementParametrName = "Z Movement";
    [SerializeField] private string _xMovementParametrName = "X Movement";
    [SerializeField] private string _moveSpeedParametrName = "Move speed";

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        CharacterInstanceReference characterPawn = _characterController.LinkedCharacter as CharacterInstanceReference;
        
        if (characterPawn == null)
        {
            Debug.LogError("CharacterInstanceReference is null");
            enabled = false;
            return;
        }
        
        _rigidbody = characterPawn.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (_rigidbody == null)
        {
            return;
        }
        
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 localVelocity = _characterController.transform.InverseTransformDirection(velocity);
        
        Vector3 animationVelocity = new(_animator.GetFloat(_xMovementParametrName), 0, _animator.GetFloat(_zMovementParametrName));
        Vector3 smoothedVelocity = Vector3.MoveTowards(animationVelocity, localVelocity, Time.deltaTime * 20);
        
        _animator.SetFloat(_zMovementParametrName, smoothedVelocity.z);
        _animator.SetFloat(_xMovementParametrName, smoothedVelocity.x);
        _animator.SetFloat(_moveSpeedParametrName, smoothedVelocity.magnitude);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterViewInstance>();
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
#endif
}