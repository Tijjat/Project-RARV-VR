using System;
using UnityEngine;


public class VRPlaceableObject : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private string id;
    [SerializeField] private string prefabId;

    public string Id => id;
    public string PrefabId => prefabId;


    public void Initialize(string prefabIdentifier)
    {
        id = "obj_" + Guid.NewGuid().ToString("N");
        prefabId = prefabIdentifier;
    }
}