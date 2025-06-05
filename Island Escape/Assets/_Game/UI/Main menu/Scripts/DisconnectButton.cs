using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class DisconnectButton : MonoBehaviour
{
    [SerializeField] private string _disconnectButtonName;
    [SerializeField] private UIDocument _document;

    private Button _button;
    
    private void Awake()
    {
        _button = _document.rootVisualElement.Q<Button>(_disconnectButtonName);
    }

    private void OnEnable()
    {
        _button.clicked += OnButtonClick;
    }

    private void OnDisable()
    {
        _button.clicked -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
