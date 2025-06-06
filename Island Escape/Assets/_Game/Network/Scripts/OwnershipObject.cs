using Unity.Netcode;
using UnityEngine;

public class OwnershipObject : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private NetworkObject _networkObject;
    [SerializeField] private bool _invert;
    private void Start()
    {
        InvokeRepeating(nameof(DelayUpdate), 0, 0.1f);
    }

    private void DelayUpdate()
    {
        _target.SetActive(_networkObject.IsOwner != _invert);
    }
}
