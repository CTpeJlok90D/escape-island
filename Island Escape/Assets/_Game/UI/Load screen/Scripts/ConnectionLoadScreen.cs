using Core.Connection;
using UnityEngine;

public class ConnectionLoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loadScreen_PREFAB;
    
    private GameObject _loadScreenInstance;

    private void OnEnable()
    {
        RelayConnection.Instance.ConnectionStarted += OnConnectionStart;
        RelayConnection.Instance.Connected += OnConnected;
    }

    private void OnDisable()
    {
        RelayConnection.Instance.ConnectionStarted -= OnConnectionStart;
        RelayConnection.Instance.Connected -= OnConnected;
    }

    private void OnConnected()
    {
        Destroy(_loadScreenInstance);
    }

    private void OnConnectionStart()
    {
        _loadScreenInstance = Instantiate(_loadScreen_PREFAB);
    }
}
