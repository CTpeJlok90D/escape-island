using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class ConnectedVisualElements : MonoBehaviour
{
    [SerializeField] private List<string> _elementLists;
    [SerializeField] private UIDocument _document;
    [SerializeField] private Mode _mode;
    
    private List<VisualElement> _visualElements = new();

    private void Start()
    {
        foreach (string elementName in _elementLists)
        {
            VisualElement visualElement = _document.rootVisualElement.Q<VisualElement>(elementName);
            _visualElements.Add(visualElement);
        }

        InvokeRepeating(nameof(UpdateVisibility),0, 0.2f);
    }

    private void UpdateVisibility()
    {
        foreach (VisualElement element in _visualElements)
        {
            if (_mode is Mode.HideOnDisconnect)
            {
                element.style.display = NetworkManager.Singleton.IsConnectedClient ? DisplayStyle.Flex : DisplayStyle.None;
            }
            else
            {
                element.style.display = NetworkManager.Singleton.IsConnectedClient == false ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }

    private enum Mode
    {
        HideOnConnect,
        HideOnDisconnect,
    }
}
