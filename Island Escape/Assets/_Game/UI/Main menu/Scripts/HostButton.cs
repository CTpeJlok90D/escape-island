using Core.Connection;
using UnityEngine;
using UnityEngine.UIElements;

public class HostButton : MonoBehaviour
{
    [SerializeField] private string _hostButtonName;
    [SerializeField] private UIDocument _document;

    private Button _button;
    
    private void Awake()
    {
        _button = _document.rootVisualElement.Q<Button>(_hostButtonName);
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
        _ = RelayConnection.Instance.CreateRelay();
    }
}
