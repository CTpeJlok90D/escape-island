using NUnit.Framework;
using UnityEngine;

public class LandformGeneratorTests
{
    [Test]
    public void GenerateLandform()
    {
        Vector2Int size = new Vector2Int(30, 30);
        LandformGenerator generator = new LandformGenerator(size);
        
        Landform landform = generator.Generate();

        Assert.AreEqual(landform.Values.Length, size.y * size.x);
    }
}