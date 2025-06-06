using System;
using Core.Entities;
using Unity.Netcode.Custom;
using UnityEngine;

public class LocationConfigContainer : NetEntity<LocationConfigContainer>
{
    public NetVariable<LocationConfig> LocationConfig { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(this);
        LocationConfig = new NetVariable<LocationConfig>();
    }
}