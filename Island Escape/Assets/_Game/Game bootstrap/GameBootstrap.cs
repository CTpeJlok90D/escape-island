using Unity.Netcode;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private LocationConfig _startLocationConfig;
    
    public void Launch()
    {
        if (NetworkManager.Singleton.IsServer == false)
        {
            throw new NotServerException("Only the server can launch game");
        }
        
        _startLocationConfig.Seed = Random.Range(int.MinValue, int.MaxValue);
        
        LocationConfigContainer.Instance.LocationConfig.Value = _startLocationConfig;
        WorldStateSwitcher.Instance.SwitchState(WorldState.Exploring);
    }
}