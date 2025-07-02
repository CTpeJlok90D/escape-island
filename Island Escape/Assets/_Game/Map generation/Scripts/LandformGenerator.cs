using UnityEngine;

public class LandformGenerator
{
    private readonly LandformGeneratorConfiguration _configuration;

    private System.Random _random;
    
    public Vector2Int Size => _configuration.Size;
    
    public LandformGenerator(LandformGeneratorConfiguration configuration)
    {
        _configuration = configuration; 
        
        _random = new(_configuration.Seed);
    }
    
    public Landform Generate()
    {
        float[,] landform = new float[Size.x, Size.y];
        Vector2Int[] positionsToGenerate = new Vector2Int[Size.x * Size.y];
        
        
        return new Landform(landform);
    }
}