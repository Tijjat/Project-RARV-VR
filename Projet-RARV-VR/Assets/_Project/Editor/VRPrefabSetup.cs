using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public static class VRPrefabSetup
{
    private const string Folder =
        "Assets/_Project/Prefabs/EditableObjects";


    [MenuItem("Factory Frenzy/Configure VR Interactables")]
    public static void ConfigureAll()
    {
        string[] guids = AssetDatabase.FindAssets(
            "t:Prefab",
            new[] { Folder }
        );


        int configured = 0;
        int skipped = 0;


        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject root = null;


            try
            {
                root = PrefabUtility.LoadPrefabContents(path);


                // 1. Add a Box Collider only if the root does not have one.
                BoxCollider box = root.GetComponent<BoxCollider>();


                if (box == null)
                {
                    box = root.AddComponent<BoxCollider>();
                    box.center = Vector3.zero;
                    box.size = Vector3.one;
                }


                // 2. Add or configure a Rigidbody on the root.
                Rigidbody rb = root.GetComponent<Rigidbody>();


                if (rb == null)
                    rb = root.AddComponent<Rigidbody>();


                rb.useGravity = false;
                rb.isKinematic = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;


                // 3. Add or configure XR Grab Interactable.
                XRGrabInteractable grab =
                    root.GetComponent<XRGrabInteractable>();


                if (grab == null)
                    grab = root.AddComponent<XRGrabInteractable>();


                grab.interactionLayers =
                    UnityEngine.XR.Interaction.Toolkit.InteractionLayerMask.GetMask(
                        "Default"
                    );


                grab.movementType =
                    XRBaseInteractable.MovementType.Kinematic;


                grab.trackPosition = true;
                grab.trackRotation = true;
                grab.throwOnDetach = false;


                // 4. Use the root Box Collider for VR selection.
                grab.colliders.Clear();
                grab.colliders.Add(box);


                // 5. Save only the editable VR prefab.
                PrefabUtility.SaveAsPrefabAsset(root, path);


                configured++;
                Debug.Log("Configured: " + path);
            }
            catch (Exception ex)
            {
                skipped++;
                Debug.LogError(
                    "Failed to configure " + path + "\n" + ex
                );
            }
            finally
            {
                if (root != null)
                    PrefabUtility.UnloadPrefabContents(root);
            }
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        Debug.Log(
            $"VR setup completed. Configured: {configured}, " +
            $"Errors: {skipped}"
        );
    }
}