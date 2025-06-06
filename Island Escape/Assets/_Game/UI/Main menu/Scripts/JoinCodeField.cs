using Core.Connection;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

public class JoinCodeField : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    [SerializeField] private string _joinCodeTextFieldName = "join-code-read-only-field";

    private TextField _joinCodeTextField;

    private void Awake()
    {
        _joinCodeTextField = _document.rootVisualElement.Q<TextField>(_joinCodeTextFieldName);
        RelayConnection.Instance.JoinCode.Subscribe(_ => OnJoinCodeChange(RelayConnection.Instance.JoinCode.Value));
    }

    private void OnJoinCodeChange(string newValue)
    {
        _joinCodeTextField.value = newValue;
    }
}
