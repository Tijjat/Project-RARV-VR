using System;
using System.Collections.Generic;
using UnityEngine;

public class VRLevelExporter : MonoBehaviour
{
    [Header("Level References")]
    [SerializeField] private Transform levelObjects;


    [Header("Level Settings")]
    [SerializeField] private string levelName = "My Factory Level";
    [SerializeField] private string levelId = "";


    [Header("Grid")]
    [SerializeField] private int gridWidth = 250;
    [SerializeField] private int gridLength = 250;
    [SerializeField] private float cellSize = 2f;
    [SerializeField]
    private Vector3 gridOrigin =
        new Vector3(-250f, 0f, -250f);


    public LevelData CollectLevel()
    {
        if (levelObjects == null)
        {
            Debug.LogError("LevelObjects is not assigned.");
            return null;
        }


        LevelData level = new LevelData
        {
            formatVersion = 1,
            levelId = levelId,
            levelName = levelName,
            isValid = false,
            grid = new LevelGrid
            {
                width = gridWidth,
                length = gridLength,
                cellSize = cellSize,
                origin = gridOrigin
            },
            objects = new List<LevelObjectData>()
        };


        VRPlaceableObject[] placedObjects =
            levelObjects.GetComponentsInChildren<VRPlaceableObject>();


        foreach (VRPlaceableObject placed in placedObjects)
        {
            if (placed == null)
                continue;


            Transform t = placed.transform;


            int rotationY =
                Mathf.RoundToInt(t.eulerAngles.y / 90f) * 90;


            rotationY = ((rotationY % 360) + 360) % 360;



            LevelObjectData data = new LevelObjectData
            {
                id = placed.Id,
                prefabId = placed.PrefabId,
                position = t.position,
                rotationY = rotationY
            };

            // Retrieve the parameters modified through the VR menu.
            VRObjectParameters objectParameters =
                placed.GetComponent<VRObjectParameters>();

            if (objectParameters != null)
            {
                data.parameters = objectParameters.Parameters;
            }
            else
            {
                // Use defaults for objects without configurable parameters.
                data.parameters = new LevelObjectParameters();
            }

            level.objects.Add(data);

        }


        Debug.Log(
            $"Collected {level.objects.Count} level objects."
        );


        return level;
    }


    [ContextMenu("Test Collect Level")]
    private void TestCollectLevel()
    {
        LevelData level = CollectLevel();


        if (level == null)
            return;


        string json = LevelJson.Serialize(level);
        Debug.Log(json);
    }

    private void Awake()
    {
        if (string.IsNullOrWhiteSpace(levelId))
            levelId = Guid.NewGuid().ToString("N");
    }
}