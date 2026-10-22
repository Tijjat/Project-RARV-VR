using System;
using System.IO;
using UnityEngine;


public class VRLevelFileManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VRLevelExporter exporter;
    [SerializeField] private VRLevelValidator validator;
    [SerializeField] private Transform levelObjects;


    [Header("Save Settings")]
    [SerializeField] private string defaultFileName = "MyFactoryLevel";


    private string MapFolder
    {
        get
        {
#if UNITY_EDITOR
            // Assets -> Unity project -> repository root
            return Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", "..", "Map")
            );
#else
            // Writable location for standalone builds / Quest
            return Path.Combine(
                Application.persistentDataPath, "Map"
            );
#endif
        }
    }

    public bool FileExists(string requestedName)
    {
        string fileName = string.IsNullOrWhiteSpace(requestedName)
            ? defaultFileName
            : requestedName.Trim();


        foreach (char c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c.ToString(), "");


        fileName = fileName.Replace("/", "").Replace("\\", "");
        fileName = fileName.Trim().TrimEnd('.');


        if (string.IsNullOrWhiteSpace(fileName))
            return false;


        if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            fileName += ".json";


        return File.Exists(Path.Combine(MapFolder, fileName));
    }

    public bool SaveLevel(string requestedName, bool overwrite = false)
    {
        if (exporter == null)
        {
            Debug.LogError("VRLevelExporter is not assigned.");
            return false;
        }


        string fileName = string.IsNullOrWhiteSpace(requestedName)
            ? defaultFileName
            : requestedName.Trim();


        // Keep filenames safe and prevent path traversal.
        foreach (char c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c.ToString(), "");


        fileName = fileName.Replace("/", "").Replace("\\", "");
        fileName = fileName.Trim().TrimEnd('.');


        if (string.IsNullOrWhiteSpace(fileName) ||
            fileName == "." || fileName == "..")
        {
            Debug.LogError("Invalid level filename.");
            return false;
        }


        if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            fileName += ".json";


        string filePath = Path.Combine(MapFolder, fileName);


        if (File.Exists(filePath) && !overwrite)
        {
            Debug.LogWarning(
                $"File already exists: {filePath}. Overwrite confirmation required."
            );
            return false;
        }


        try
        {
            LevelData level = exporter.CollectLevel();


            if (level == null)
                return false;


            if (validator == null || levelObjects == null)
            {
                Debug.LogError("Validator or LevelObjects reference is missing");
                return false;
            }

            validator.Validate(level, levelObjects);


            // Set a readable level name.
            level.levelName = Path.GetFileNameWithoutExtension(fileName);


            string json = LevelJson.Serialize(level);


            Directory.CreateDirectory(MapFolder);
            File.WriteAllText(filePath, json);

            VRAudioManager.Instance?.PlaySave();

            Debug.Log($"Level saved successfully: {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save level: {ex}");
            return false;
        }
    }


    [ContextMenu("Test Save Level")]
    private void TestSaveLevel()
    {
        SaveLevel(defaultFileName);
    }
}