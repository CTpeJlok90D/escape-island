using UnityEngine;

public class CharacterCameraXMovement : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Transform _cameraTransform;

    private void Update()
    {
        _cameraTransform.localEulerAngles = new Vector3(_characterMovement.Rotation.Value.x, 0, 0);
    }
}
