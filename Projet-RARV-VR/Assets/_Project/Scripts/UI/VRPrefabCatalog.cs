using System;
using System.Collections.Generic;
using UnityEngine;


public enum VRPrefabCategory
{
    Essentials,
    Platforms,
    Traps
}


[Serializable]
public class VRPrefabEntry
{
    [Header("Display")]
    public string displayName;
    public VRPrefabCategory category;
    public Sprite previewImage;


    [Header("Prefab")]
    public GameObject prefab;


    [Header("Shared JSON")]
    public string prefabId;
}


public class VRPrefabCatalog : MonoBehaviour
{
    [SerializeField]
    private List<VRPrefabEntry> entries = new();


    public IReadOnlyList<VRPrefabEntry> Entries => entries;


    public List<VRPrefabEntry> GetByCategory(
        VRPrefabCategory category)
    {
        return entries.FindAll(
            entry => entry.category == category
                     && entry.prefab != null
        );
    }


    public VRPrefabEntry GetById(string prefabId)
    {
        return entries.Find(
            entry => entry.prefabId == prefabId
        );
    }


    private void OnValidate()
    {
        HashSet<string> usedIds = new();


        foreach (VRPrefabEntry entry in entries)
        {
            if (entry == null)
                continue;


            if (string.IsNullOrWhiteSpace(entry.prefabId))
            {
                Debug.LogWarning(
                    "A catalog entry has no prefabId.",
                    this
                );
                continue;
            }


            if (!usedIds.Add(entry.prefabId))
            {
                Debug.LogWarning(
                    "Duplicate prefabId: " + entry.prefabId,
                    this
                );
            }
        }
    }
}