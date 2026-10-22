using System.Collections.Generic;
using UnityEngine;


public class VRObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform spawnedObjectsParent;
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float spawnHeight = 1.0f;


    // Associates spawned objects with their JSON prefabId.
    private readonly Dictionary<GameObject, string> spawnedObjects = new();


    public void Spawn(VRPrefabEntry entry)
    {
        if (entry == null || entry.prefab == null)
        {
            Debug.LogWarning("Cannot spawn: missing prefab.");
            return;
        }


        if (playerCamera == null)
        {
            Debug.LogError("Cannot spawn: player camera is missing.");
            return;
        }


        // Start and Finish must be unique.
        bool isUnique =
            entry.prefabId == "platform_start" ||
            entry.prefabId == "platform_finish";


        if (isUnique && AlreadyExists(entry.prefabId))
        {
            Debug.LogWarning(
                $"Cannot create another {entry.displayName}: " +
                "only one instance is allowed per level."
            );
            return;
        }

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 spawnPosition =
            playerCamera.position + forward * spawnDistance;

        spawnPosition.y = spawnHeight;


        Quaternion spawnRotation = Quaternion.identity;


        GameObject spawnedObject = Instantiate(
            entry.prefab,
            spawnPosition,
            spawnRotation,
            spawnedObjectsParent
        );


        spawnedObject.name = entry.displayName;


        VRPlaceableObject placeable = spawnedObject.GetComponent<VRPlaceableObject>();

        if (placeable == null)
        {
            placeable = spawnedObject.AddComponent<VRPlaceableObject>();
        }

        placeable.Initialize(entry.prefabId);

        // Ensure every spawned object respects the build area.
        if (spawnedObject.GetComponent<VRBuildBoundsObject>() == null)
        {
            spawnedObject.AddComponent<VRBuildBoundsObject>();
        }

        VRBuildBounds buildBounds = FindAnyObjectByType<VRBuildBounds>();

        if (buildBounds != null)
        {
            buildBounds.ClampObject(spawnedObject);
        }
        else
        {
            Debug.LogWarning("VRBuildBounds was not found in the scene");
        }

            spawnedObjects.Add(spawnedObject, entry.prefabId);


        Debug.Log(
            $"Spawned: {entry.displayName} ({entry.prefabId})"
        );
    }


    private bool AlreadyExists(string prefabId)
    {
        // Remove references to objects that have been destroyed.
        List<GameObject> destroyedObjects = new();


        foreach (var item in spawnedObjects)
        {
            if (item.Key == null)
            {
                destroyedObjects.Add(item.Key);
                continue;
            }


            if (item.Value == prefabId)
                return true;
        }


        foreach (GameObject obj in destroyedObjects)
            spawnedObjects.Remove(obj);


        return false;
    }
}