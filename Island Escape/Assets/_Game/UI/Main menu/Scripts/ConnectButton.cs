using Core.Connection;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectButton : MonoBehaviour
{
    [SerializeField] private string _connectButtonName;
    [SerializeField] private string _joinCodeFieldName;
    [SerializeField] private UIDocument _document;

    private Button _button;
    private TextField _joinCodeField;
    
    private void Awake()
    {
        _button = _document.rootVisualElement.Q<Button>(_connectButtonName);
        _joinCodeField = _document.rootVisualElement.Q<TextField>(_joinCodeFieldName);
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
        _ = RelayConnection.Instance.JoinRelay(_joinCodeField.value);
    }
}
