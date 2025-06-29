using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteractionSystem : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputActionReference _interactionActionReference;
    [SerializeField] private float _interactionDistance = 1.7f;

    public InputAction InteractionAction => _interactionActionReference.action;

    private void Awake()
    {
        InteractionAction.Enable();
    }

    private void OnEnable()
    {
        InteractionAction.canceled += OnInteractionCancel;
    }

    private void OnDisable()
    {
        InteractionAction.canceled -= OnInteractionCancel;
    }

    private void OnInteractionCancel(InputAction.CallbackContext obj)
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray screenRay = _camera.ScreenPointToRay(screenCenter);
        
        if (Physics.Raycast(screenRay, out RaycastHit hitInfo, _interactionDistance) && hitInfo.collider.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact(_root);
        }
    }
}
