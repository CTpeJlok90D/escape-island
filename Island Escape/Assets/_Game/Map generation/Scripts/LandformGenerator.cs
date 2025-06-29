using UnityEngine;

public class LandformGenerator
{
    private const float NoiseScale = 0.01f;
    private const int LandformScale = 10;
    private readonly Vector2Int _size;
    private readonly Vector2Int _offset;
    private readonly int _seed;
    private readonly int _defaultOffset;
    
    public LandformGenerator(Vector2Int size, int seed = -1)
    {
        _size = size;
        _seed = seed;

        if (_seed == -1)
        {
            _seed = Random.Range(int.MinValue, int.MaxValue);
        }

        System.Random random = new(_seed);
        _offset = new Vector2Int()
        {
            x = random.Next(0, int.MaxValue),
            y = random.Next(0, int.MaxValue)
        };
    }
    
    public Landform Generate()
    {
        int[,] landform = new int[_size.x, _size.y];

        string result = "";
        
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                float noiseHeight = Mathf.PerlinNoise((x + _offset.x) * NoiseScale, (y + _offset.y) * NoiseScale);
                landform[x,y] = (int)(noiseHeight * LandformScale) - _defaultOffset;
            }

            result += "\n";
        }
        
        return new Landform(landform);
    }
}