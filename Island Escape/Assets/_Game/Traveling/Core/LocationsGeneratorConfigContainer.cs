using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core
{
    public class LocationsGeneratorConfigContainer : NetEntity<LocationsGeneratorConfigContainer>
    {
        [SerializeField] private LocationsGeneratorConfig _defaultConfig;
        
        public NetVariable<LocationsGeneratorConfig> Config { get; private set; }

        public override void Awake()
        {
            base.Awake();
            Config = new(_defaultConfig);
        }
    }
}