using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CharactersSpawner : MonoBehaviour
{
    [SerializeField] private CharacterInstanceReference _characterInstanceReference;
    
    private void Start()
    {
        if (NetworkManager.Singleton == null || NetworkManager.Singleton.IsServer == false)
        {
            return;
        }
        
        List<PlayerSpawnPoint> spawnPoints = PlayerSpawnPoint.Instances.ToList();
        
        foreach (CharacterInstance instance in CharacterInstance.Instances.Where(x => x.IsBot.Value == false))
        {
            PlayerSpawnPoint spawnPoint = spawnPoints.PopRandom();
            
            _characterInstanceReference.Instantiate(instance, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}