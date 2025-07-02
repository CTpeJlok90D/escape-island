using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;
using Random = System.Random;

[DefaultExecutionOrder(-1)]
public class MapGeneratorConfigurationContainer : NetEntity<MapGeneratorConfigurationContainer>
{
    [SerializeField] private MapGeneratorConfiguration _defaultMapConfiguration;
    
    public NetVariable<MapGeneratorConfiguration> Config { get; private set; }
    public Random Random { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Config = new(_defaultMapConfiguration);
    }

    public Random CreateRandom()
    {
        Random = new(Config.Value.Seed);
        return Random;
    }
}