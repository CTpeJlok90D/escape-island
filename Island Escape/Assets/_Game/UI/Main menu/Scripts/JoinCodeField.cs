using System;
using Core.Connection;
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
    }

    private void OnEnable()
    {
        RelayConnection.Instance.JoinCode.Changed += OnJoinCodeChange;
    }

    private void OnDisable()
    {
        RelayConnection.Instance.JoinCode.Changed -= OnJoinCodeChange;
    }

    private void OnJoinCodeChange(string oldValue, string newValue)
    {
        _joinCodeTextField.value = newValue;
    }
}
