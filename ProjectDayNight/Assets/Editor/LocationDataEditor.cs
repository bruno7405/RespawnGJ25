using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LocationData))]
public class LocationDataEditor : Editor
{
    private readonly List<GameObject> sceneObjects = new();
    private int amountOfLocations = 0;
    public override void OnInspectorGUI()
    {
        LocationData data = (LocationData)target;

        amountOfLocations = EditorGUILayout.IntField("Amount of Locations", amountOfLocations);

        while (sceneObjects.Count < amountOfLocations) sceneObjects.Add(null);
        while (sceneObjects.Count > amountOfLocations) sceneObjects.RemoveAt(sceneObjects.Count - 1);
        
        EditorGUILayout.LabelField("Scene Objects (Editor Only)", EditorStyles.boldLabel);
        for (int i = 0; i < amountOfLocations; i++)
        {
            EditorGUILayout.BeginHorizontal();
            sceneObjects[i] = (GameObject)EditorGUILayout.ObjectField("Object " + i, sceneObjects[i], typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();
        }

        // Bake Button
        if (GUILayout.Button("Bake Positions"))
        {
            GenerateFromSceneObjects(data);
        }

        // Show baked positions
        EditorGUILayout.LabelField("Baked Positions:", EditorStyles.boldLabel);
        foreach (KeyValuePair<string, Vector2> kvp in data.BakedMap)
        {
            EditorGUILayout.Vector2Field(kvp.Key, kvp.Value);
        }
    }

    private void GenerateFromSceneObjects(LocationData data)
    {
        data.SetLocationEntries(sceneObjects.ConvertAll(go => new LocationEntry { name = go.name, position = go.transform.position }));

        EditorUtility.SetDirty(data);
        Debug.Log("LocationData generated from scene objects!");

        AssetDatabase.SaveAssets();
    }
}