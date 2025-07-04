using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;

namespace Core
{
    public class GameEventsGeneratorConfigContainer : NetEntity<GameEventsGeneratorConfigContainer>
    {
        [SerializeField] private GameEventsGeneratorConfig _defaultConfig;
        
        public NetVariable<GameEventsGeneratorConfig> Config { get; private set; }

        public override void Awake()
        {
            base.Awake();
            Config = new(_defaultConfig);
        }
    }
}