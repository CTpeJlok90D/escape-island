using System;
using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocationsGeneratorConfigContainer : NetEntity<LocationsGeneratorConfigContainer>
{
    [SerializeField] private LocationsGeneratorConfig _defaultConfig;
    
    public NetVariable<LocationsGeneratorConfig> Config { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Config = new NetVariable<LocationsGeneratorConfig>(_defaultConfig);
        Config.Value.Seed = Random.Range(Int32.MinValue, Int32.MaxValue);
    }
}