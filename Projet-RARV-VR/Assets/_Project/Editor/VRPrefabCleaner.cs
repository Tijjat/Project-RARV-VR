using System.IO;
using UnityEditor;
using UnityEngine;


public static class VRPrefabCleaner
{
    private const string SourceFolder =
        "Assets/Prefabs/Shared";


    private const string DestinationFolder =
        "Assets/_Project/Prefabs/EditableObjects";


    [MenuItem("Factory Frenzy/Generate VR Prefabs")]
    public static void GenerateVRPrefabs()
    {
        if (!AssetDatabase.IsValidFolder(SourceFolder))
        {
            Debug.LogError("Source folder not found: " + SourceFolder);
            return;
        }


        // Create destination folders if necessary.
        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Prefabs");
        EnsureFolder(DestinationFolder);


        string[] prefabGuids = AssetDatabase.FindAssets(
            "t:Prefab",
            new[] { SourceFolder }
        );


        int created = 0;
        int skipped = 0;


        foreach (string guid in prefabGuids)
        {
            string sourcePath = AssetDatabase.GUIDToAssetPath(guid);


            GameObject sourcePrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);


            if (sourcePrefab == null)
                continue;


            string outputName = "VR_" + sourcePrefab.name;
            string outputPath = DestinationFolder + "/" +
                                outputName + ".prefab";


            // Never overwrite an existing VR prefab.
            if (File.Exists(outputPath))
            {
                Debug.Log("Skipped existing prefab: " + outputPath);
                skipped++;
                continue;
            }


            GameObject instance = null;


            try
            {
                // Instantiate, then remove every prefab connection.
                instance = (GameObject)PrefabUtility.InstantiatePrefab(
                    sourcePrefab
                );


                PrefabUtility.UnpackPrefabInstance(
                    instance,
                    PrefabUnpackMode.Completely,
                    InteractionMode.AutomatedAction
                );


                int removedComponents = 0;


                foreach (Transform child in
                         instance.GetComponentsInChildren<Transform>(true))
                {
                    removedComponents +=
                        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(
                            child.gameObject
                        );
                }


                // Remove only missing MonoBehaviour references.
                // Preserve all models, colliders and other components.
                PrefabUtility.SaveAsPrefabAsset(instance, outputPath);


                Debug.Log(
                    "Created: " + outputPath +
                    " | Missing scripts removed: " + removedComponents
                );


                created++;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    "Failed to process " + sourcePath + ": " + ex.Message
                );
            }
            finally
            {
                if (instance != null)
                    Object.DestroyImmediate(instance);
            }
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        Debug.Log(
            "VR prefab generation completed. " +
            "Created: " + created +
            ", skipped: " + skipped
        );
    }


    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
            return;


        string parent = Path.GetDirectoryName(folderPath)
            .Replace("\\", "/");


        string folderName = Path.GetFileName(folderPath);


        if (!AssetDatabase.IsValidFolder(parent))
            EnsureFolder(parent);


        AssetDatabase.CreateFolder(parent, folderName);
    }
}
