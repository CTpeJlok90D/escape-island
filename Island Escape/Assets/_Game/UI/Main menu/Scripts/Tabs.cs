using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UIElements;

public class Tabs : MonoBehaviour
{
    [SerializedDictionary("Button", "Tab name")]
    [SerializeField] private SerializedDictionary<string, string> _tabs;
    [SerializeField] private UIDocument _document;
    
    private Dictionary<Button, VisualElement> _buttonAndLinkedTabs = new();

    private void Start()
    {
        foreach (KeyValuePair<string, string> tabs in _tabs)
        {
            Button button = _document.rootVisualElement.Q<Button>(tabs.Key);
            VisualElement linkedTab = _document.rootVisualElement.Q<VisualElement>(tabs.Value);
            
            linkedTab.style.visibility = Visibility.Hidden;
            _buttonAndLinkedTabs.Add(button, linkedTab);

            Debug.Log($"{button.name} and {linkedTab.name}");
            
            if (button == null)
            {
                continue;
            }
            
            button.RegisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnButtonClick(ClickEvent evt)
    {
        Button clickedButton = evt.target as Button;
        VisualElement newTab = _buttonAndLinkedTabs[clickedButton];
        
        foreach ((Button button, VisualElement tab) in _buttonAndLinkedTabs)
        {
            button.SetEnabled(true);
            if (tab != null)
            {
                tab.style.visibility = Visibility.Hidden;
            }
        }
        
        if (newTab != null)
        {
            clickedButton.SetEnabled(false);
            newTab.style.visibility = Visibility.Visible;
        }
    }

    public void EnableTab(int tabIndex)
    {
        (Button linkedButton, VisualElement newTab) = _buttonAndLinkedTabs.ElementAt(tabIndex);
        
        foreach ((Button button, VisualElement tab) in _buttonAndLinkedTabs)
        {
            button.SetEnabled(true);
            if (tab != null)
            {
                tab.style.visibility = Visibility.Hidden;
            }
        }
        
        if (newTab != null)
        {
            linkedButton.SetEnabled(false);
            newTab.style.visibility = Visibility.Visible;
        }
    }
}
