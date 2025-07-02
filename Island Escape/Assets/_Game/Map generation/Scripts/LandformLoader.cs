using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Custom;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR 
using UnityEditor;
using Progress = UnityEditor.Progress;
#endif

namespace Core
{
    public class LandformLoader : NetworkBehaviour
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private LandformGeneratorConfiguration _configuration;

        private NetVariable<int> _seed;
        public bool IsInProgress { get; private set; } 
        
#if UNITY_EDITOR
        private int _processId;
#endif
        
        private void Awake()
        {
            _seed = new(Random.Range(int.MinValue, int.MaxValue));
        }

        private void Start()
        {
            _ = GenerateLandform(_seed.Value);
        }

        private async UniTask GenerateLandform(int seed)
        {
            if (IsInProgress)
            {
                throw new InvalidOperationException("Generation is already in progress");
            }
            
            IsInProgress = true;
#if UNITY_EDITOR
            _processId = Progress.Start("Generating map");
#endif
            
            TerrainData terrainData = Instantiate(_terrain.terrainData);
            int resolution = terrainData.heightmapResolution;

            _configuration = new()
            {
                Size = new(terrainData.heightmapResolution, terrainData.heightmapResolution),
                Seed = seed,
            };
            LandformGenerator generator = new(_configuration);
            Landform landform = generator.Generate();

            float[,] heights = new float[resolution, resolution];
            
            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    float height = landform.Values[x, y] / _configuration.MaxHeight;
                    heights[x, y] = height;
                }
                
                await UniTask.NextFrame();
#if UNITY_EDITOR
                Progress.Report(_processId, (float)x/resolution);
#endif
            }
            
            terrainData.SetHeights(0, 0, heights);
            _terrain.terrainData = terrainData;
            
#if UNITY_EDITOR
            Progress.Finish(_processId);
#endif
            IsInProgress = false;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            if (Progress.Exists(_processId))
            {
                Progress.Finish(_processId);
            }
#endif
        }

        [ContextMenu("Regenerate")]
        private void Regenerate()
        {
            if (NetworkManager.Singleton == null)
            {
                int seed = Random.Range(int.MinValue, int.MaxValue);
                _ = GenerateLandform(seed);

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
            _ = GenerateLandform(seed);
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(LandformLoader))]
        private class CEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                if (Application.IsPlaying(this) == false)
                {
                    return;
                }
                
                LandformLoader loader = (LandformLoader)target;
                if (GUILayout.Button("Regenerate"))
                {
                    loader.Regenerate();
                }
            }
        }
#endif
    }
}