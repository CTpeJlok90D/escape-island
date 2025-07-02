using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public record LandformGeneratorConfiguration
{
    public float MaxHeight = 8;
    public Vector2Int Size = new (100, 100);
    public int Seed = -1;
    public int PlatformCount = 6;
    public int PlatformMaxSize = 8;
    public int PlatformMinSize = 3;

    public LandformGeneratorConfiguration(int seed = -1)
    {
        if (seed == -1)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Seed = seed;
    }
}