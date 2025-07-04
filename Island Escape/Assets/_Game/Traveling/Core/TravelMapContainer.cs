using System.Linq;
using Core.Entities;
using Unity.Netcode.Custom;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class TravelMapContainer : NetEntity<TravelMapContainer>
    {
        private NetVariable<TravelMap> _travelMap;
        public LocationsGeneratorConfig Config => LocationsGeneratorConfigContainer.Instance.Config.Value;
        public TravelMap TravelMap => _travelMap.Value;
        
        public delegate void GeneratedDelegate(TravelMapContainer sender, TravelMap travelMap);
        public static event GeneratedDelegate Generated;
        
        public override void Awake()
        {
            base.Awake();
            _travelMap = new NetVariable<TravelMap>();
        }
        
        public void Generate()
        {
            TravelMapGenerator generator = new(Config);
            _travelMap.Value = generator.Generate();
            Generated?.Invoke(this, _travelMap.Value);
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(TravelMapContainer))]
        private class CEditor : Editor
        {
            private new TravelMapContainer target => base.target as TravelMapContainer;
            
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (Application.IsPlaying(target) == false)
                {
                    return;
                }

                if (GUILayout.Button("Generate"))
                {
                    target.Generate();
                }
            }
        }
#endif
    }
}