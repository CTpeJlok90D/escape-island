using Unity.Netcode;
using UnityEngine;

public class OwnershipObject : NetworkBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private NetworkObject _networkObject;
    [SerializeField] private bool _invert;

    private void Start()
    {
        UpdateView();
    }
    
    private void UpdateView()
    {
        _target.SetActive(_networkObject.IsOwner != _invert);
    }

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        UpdateView();
    }

    public override void OnLostOwnership()
    {
        base.OnLostOwnership();
        UpdateView();
    }
}
