
using UnityEngine;
using UnityEngine.XR;

public class VRObjectMenuInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private VRObjectSettingsMenu settingsMenu;

    [Header("Raycast Settings")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask objectLayers = ~0;

    [Header("Menu Position")]
    [SerializeField] private float menuDistance = 1.2f;

    private InputDevice leftController;
    private bool previousXState;

    private void Update()
    {
        if (!leftController.isValid)
        {
            leftController =
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        if (!leftController.isValid)
            return;

        if (!leftController.TryGetFeatureValue(
            CommonUsages.primaryButton,
            out bool xPressed))
        {
            return;
        }

        // Detect a single press of X.
        if (xPressed && !previousXState)
        {
            ToggleMenu();
        }

        previousXState = xPressed;
    }


    private void ToggleMenu()
    {
        if (settingsMenu == null)
            return;

        if (settingsMenu.IsOpen)
        {
            settingsMenu.Close();
            return;
        }

        if (playerCamera == null)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            maxDistance,
            objectLayers,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log("No object targeted.");
            return;
        }

        VRPlaceableObject target =
            hit.collider.GetComponentInParent<VRPlaceableObject>();

        if (target == null)
            return;

        PositionMenu();
        settingsMenu.Open(target);
    }


    private void PositionMenu()
    {
        Transform cameraTransform = playerCamera.transform;

        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Transform menuTransform = settingsMenu.transform;

        menuTransform.position =
            cameraTransform.position +
            forward * menuDistance;

        menuTransform.rotation =
            Quaternion.LookRotation(
                menuTransform.position -
                cameraTransform.position);
    }
}
