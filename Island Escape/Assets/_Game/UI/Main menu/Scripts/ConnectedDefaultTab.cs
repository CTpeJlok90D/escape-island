using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectedDefaultTab : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private int _connectedTabIndex;
    [SerializeField] private Tabs _tabs;

    private void Start()
    {
        NetworkManager.Singleton.OnClientStarted += OnClientStart;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted -= OnClientStart;
        }
    }

    private void OnClientStart()
    {
        _tabs.EnableTab(_connectedTabIndex);
    }
}
