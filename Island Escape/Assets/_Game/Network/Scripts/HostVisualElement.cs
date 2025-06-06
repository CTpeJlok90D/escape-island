using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class HostVisualElement : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private List<string> _elementNames;

    private List<VisualElement> _visualElements;
    
    private void Start()
    {
        foreach (string elementName in _elementNames)
        {
            VisualElement visualElement = _uiDocument.rootVisualElement.Q<VisualElement>(elementName);
            _visualElements.Add(visualElement);
        }
        
        InvokeRepeating(nameof(DelayUpdate), 0, 0.1f);
    }

    private void DelayUpdate()
    {
        foreach (VisualElement element in _visualElements)
        {
            element.style.display = NetworkManager.Singleton.IsHost ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
