using System.Collections.Generic;
using UnityEngine;


public class VRLevelValidator : MonoBehaviour
{
    [Header("Validation Rules")]
    [SerializeField] private int minAdditionalObjects = 10;


    [Header("Construction Limits")]
    [SerializeField] private float minX = -235f;
    [SerializeField] private float maxX = 235f;
    [SerializeField] private float minZ = -235f;
    [SerializeField] private float maxZ = 235f;


    public bool Validate(LevelData level, Transform levelObjects)
    {
        if (level == null || levelObjects == null)
            return false;


        List<string> errors = new List<string>();


        int starts = 0;
        int finishes = 0;
        int checkpoints = 0;
        int additionalObjects = 0;


        HashSet<string> ids = new HashSet<string>();


        foreach (LevelObjectData obj in level.objects)
        {
            if (obj == null)
            {
                errors.Add("Null object in level.");
                continue;
            }


            if (string.IsNullOrWhiteSpace(obj.id) ||
                !ids.Add(obj.id))
            {
                errors.Add("Missing or duplicate object ID.");
            }


            if (string.IsNullOrWhiteSpace(obj.prefabId))
            {
                errors.Add("Missing prefabId.");
                continue;
            }


            switch (obj.prefabId)
            {
                case "platform_start":
                    starts++;
                    break;


                case "platform_finish":
                    finishes++;
                    break;


                case "checkpoint":
                    checkpoints++;
                    additionalObjects++;
                    break;


                default:
                    if (!obj.prefabId.StartsWith("decoration_"))
                        additionalObjects++;
                    break;
            }


            if (obj.rotationY != 0 &&
                obj.rotationY != 90 &&
                obj.rotationY != 180 &&
                obj.rotationY != 270)
            {
                errors.Add("Invalid rotation: " + obj.id);
            }


            Vector3 p = obj.position;


            if (!IsFinite(p.x) ||
                !IsFinite(p.y) ||
                !IsFinite(p.z))
            {
                errors.Add("Invalid position: " + obj.id);
            }
        }


        if (starts != 1)
            errors.Add("Exactly 1 Start required. Found: " + starts);


        if (finishes != 1)
            errors.Add("Exactly 1 Finish required. Found: " + finishes);


        if (checkpoints < 1)
            errors.Add("At least 1 Checkpoint required.");


        if (additionalObjects < minAdditionalObjects)
        {
            errors.Add(
                $"At least {minAdditionalObjects} additional " +
                $"gameplay objects required. Found: {additionalObjects}"
            );
        }


        // Check complete collider bounds, not only object pivots.
        VRPlaceableObject[] placedObjects =
            levelObjects.GetComponentsInChildren<VRPlaceableObject>();


        foreach (VRPlaceableObject placed in placedObjects)
        {
            Collider[] colliders =
                placed.GetComponentsInChildren<Collider>();


            bool hasBounds = false;
            Bounds combined = new Bounds();


            foreach (Collider col in colliders)
            {
                if (!col.enabled || col.isTrigger)
                    continue;


                if (!hasBounds)
                {
                    combined = col.bounds;
                    hasBounds = true;
                }
                else
                {
                    combined.Encapsulate(col.bounds);
                }
            }


            if (!hasBounds)
            {
                errors.Add("Missing collider: " + placed.name);
                continue;
            }


            if (combined.min.x < minX ||
                combined.max.x > maxX ||
                combined.min.z < minZ ||
                combined.max.z > maxZ)
            {
                errors.Add("Outside construction limits: " + placed.name);
            }
        }


        bool isValid = errors.Count == 0;
        level.isValid = isValid;


        if (isValid)
        {
            Debug.Log(
                $"Level VALID: {level.objects.Count} objects, " +
                $"{additionalObjects} additional gameplay objects."
            );
        }
        else
        {
            Debug.LogWarning(
                "Level INVALID:\n- " +
                string.Join("\n- ", errors)
            );
        }


        return isValid;
    }


    private bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}