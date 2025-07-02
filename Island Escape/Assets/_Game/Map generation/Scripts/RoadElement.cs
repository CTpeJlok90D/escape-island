using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core.Entities;
using UnityEngine;

public class RoadElement : Entity<RoadElement>
{
    
    [SerializeField] private SerializedDictionary<Transform, RoadElementList> _generation;
    public IReadOnlyDictionary<Transform, RoadElementList> Generation => _generation;

    public List<RoadElement> LinkedRoads { get; private set; } = new();
    public RoadElement Parent;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach ((Transform targetTransform, RoadElementList list) in Generation)
        {
            Gizmos.DrawLine(targetTransform.position, transform.position);
        }
    }
#endif
}