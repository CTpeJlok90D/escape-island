using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class LandformLoader : NetworkBehaviour
    {
        [SerializeField] private GameObject _landformPart;
        [SerializeField] private Vector2Int _size = new (100, 100);
        [SerializeField] private Transform _generationRoot;

        private NetVariable<int> _seed;

        private List<GameObject> _generated = new();
        
        private void Awake()
        {
            _seed = new(Random.Range(int.MinValue, int.MaxValue));
        }

        private void Start()
        {
            GenerateLandform(_seed.Value);
        }

        private void GenerateLandform(int seed)
        {
            LandformGenerator generator = new(_size, seed);
            Landform landform = generator.Generate();

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    int height = landform.Values[x, y];
                    Vector3 position = new Vector3(x - _size.x, height, y - _size.x);

                    GameObject instance = Instantiate(_landformPart, position, Quaternion.identity);
                    instance.transform.SetParent(_generationRoot);
                    _generated.Add(instance);
                }
            }
        }

        public void DestroyGenerated()
        {
            foreach (GameObject gameObject in _generated)
            {
                Destroy(gameObject);
            }
        }

        [ContextMenu("Regenerate")]
        private void Regenerate()
        {
            if (NetworkManager.Singleton == null)
            {
                int seed = Random.Range(int.MinValue, int.MaxValue);
                DestroyGenerated();
                GenerateLandform(seed);

                return;
            }
            
            if (NetworkManager.Singleton.IsServer == false)
            {
                Debug.LogError("Only server can regenerate world");
                return;
            }
            
            _seed.Value = Random.Range(int.MinValue, int.MaxValue);
            Regenerate_RPC(_seed.Value);
        }

        [Rpc(SendTo.Everyone)]
        private void Regenerate_RPC(int seed)
        {
            DestroyGenerated();
            GenerateLandform(seed);
        }
    }
}