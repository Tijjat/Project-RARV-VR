using UnityEngine;


public class VRBuildBounds : MonoBehaviour
{
    [Header("Build Area - World Coordinates")]
    [SerializeField] private float minX = -235f;
    [SerializeField] private float maxX = 235f;
    [SerializeField] private float minZ = -235f;
    [SerializeField] private float maxZ = 235f;


    public void ClampObject(GameObject target)
    {
        if (target == null)
            return;


        Collider[] colliders =
            target.GetComponentsInChildren<Collider>();


        Bounds combinedBounds = new Bounds();
        bool hasBounds = false;


        foreach (Collider col in colliders)
        {
            if (!col.enabled || col.isTrigger)
                continue;


            if (!hasBounds)
            {
                combinedBounds = col.bounds;
                hasBounds = true;
            }
            else
            {
                combinedBounds.Encapsulate(col.bounds);
            }
        }


        if (!hasBounds)
        {
            Debug.LogWarning(
                $"No valid Collider found on {target.name}."
            );
            return;
        }


        Vector3 correction = Vector3.zero;


        // Center an oversized object on the corresponding axis.
        if (combinedBounds.size.x > maxX - minX)
        {
            correction.x =
                (minX + maxX) * 0.5f - combinedBounds.center.x;
        }
        else if (combinedBounds.min.x < minX)
        {
            correction.x = minX - combinedBounds.min.x;
        }
        else if (combinedBounds.max.x > maxX)
        {
            correction.x = maxX - combinedBounds.max.x;
        }


        if (combinedBounds.size.z > maxZ - minZ)
        {
            correction.z =
                (minZ + maxZ) * 0.5f - combinedBounds.center.z;
        }
        else if (combinedBounds.min.z < minZ)
        {
            correction.z = minZ - combinedBounds.min.z;
        }
        else if (combinedBounds.max.z > maxZ)
        {
            correction.z = maxZ - combinedBounds.max.z;
        }


        if (correction.sqrMagnitude > 0.000001f)
        {
            Rigidbody rb = target.GetComponent<Rigidbody>();


            Vector3 correctedPosition =
                target.transform.position + correction;


            if (rb != null)
            {
                rb.position = correctedPosition;
            }
            else
            {
                target.transform.position = correctedPosition;
            }


            Debug.Log(
                $"{target.name}: corrected to stay inside build bounds."
            );
        }
    }
}