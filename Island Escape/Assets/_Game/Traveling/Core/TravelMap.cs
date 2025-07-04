using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public struct TravelMap : INetworkSerializable
    {
        public Vector2Int Size { get; private set; }
        private Location[] _locations;
        public const float OneCellSize = 18_000f;
        
        public IReadOnlyCollection<Location> Locations => _locations;
        public bool IsEmpty => _locations == null || _locations.Length == 0;
        
        public TravelMap(Vector2Int size)
        {
            Size = size;
            _locations = Array.Empty<Location>();
        }

        public void AddLocation(Location location)
        {
            if (location.Position.x > Size.x || location.Position.y > Size.y)
            {
                throw new IndexOutOfRangeException($"Location with position {location.Position} is out of map range");
            }

            List<Location> newLocationList = _locations.ToList(); 
            newLocationList.Add(location);
            _locations = newLocationList.ToArray();
        }

        public void AddLocations(IEnumerable<Location> locations)
        {
            foreach (Location location in locations)
            {
                AddLocation(location);
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            // Size serialization
            int sizeX = Size.x;
            int sizeY = Size.y;
            serializer.SerializeValue(ref sizeX);
            serializer.SerializeValue(ref sizeY);
            Size = new(sizeX, sizeY);

            // Locations serialization
            int locationsCount = _locations.Length;
            serializer.SerializeValue(ref locationsCount);
            if (serializer.IsReader)
            {
                _locations = new Location[locationsCount];
            }
            for (int i = 0; i < locationsCount; i++)
            {
                serializer.SerializeValue(ref _locations[i]);
            }
        }
    }
}