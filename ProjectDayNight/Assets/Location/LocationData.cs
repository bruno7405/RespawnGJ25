using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct LocationEntry
{
    public string name;
    public Vector3 position;
}

[CreateAssetMenu(fileName = "LocationData", menuName = "Locations/Location Map")]
public class LocationData : ScriptableObject
{
    private List<LocationEntry> locationEntries;

    private Dictionary<string, Vector2> _bakedMap;

    public IReadOnlyDictionary<string, Vector2> BakedMap
    {
        get
        {
            if (_bakedMap == null)
            {
                _bakedMap = new Dictionary<string, Vector2>();
                foreach (LocationEntry entry in locationEntries)
                    if (!string.IsNullOrEmpty(entry.name))
                        _bakedMap[entry.name] = entry.position;
            }
            return _bakedMap;
        }
    }

    private static LocationData _instance;
    public static LocationData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<LocationData>("LocationData");
            return _instance;
        }
    }

    public void SetLocationEntries(List<LocationEntry> entries)
    {
        locationEntries = entries;
        _bakedMap = null;
    }
}