using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKFoot : MonoBehaviour
{
    [SerializeField] private AvatarIKGoal _ikBoneRight = AvatarIKGoal.LeftFoot;
    [SerializeField] private AvatarIKGoal _ikBoneLeft = AvatarIKGoal.RightFoot;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _rayCastMask;
    [SerializeField] private float _maxDistanceToGround = 0.1f;
    [SerializeField] private Vector3 _footOffcet;

    private void OnAnimatorIK(int layerIndex)
    {
        IkFoot(_ikBoneRight);
        IkFoot(_ikBoneLeft);
    }

    private void IkFoot(AvatarIKGoal foot)
    {
        Vector3 ikBonePosition = _animator.GetIKPosition(foot);
        Ray ray = new Ray(ikBonePosition + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _rayCastMask))
        {
            _animator.SetIKPositionWeight(foot, 1);
            Vector3 hitPosition = hit.point;
            if (Vector3.Distance(hitPosition, ikBonePosition) > _maxDistanceToGround)
            {
                return;
            }

            Quaternion footRotation = Quaternion.LookRotation(transform.forward, hit.normal);
            _animator.SetIKPosition(foot, hitPosition + _footOffcet);
            _animator.SetIKRotation(foot, footRotation);
        }
        else
        {
            _animator.SetIKPositionWeight(foot, 0);
        }
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
#endif
}