using UnityEngine;
using UnityEngine.XR;


public class VRSpawnMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject spawnMenu;


    [Header("Menu Position")]
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float horizontalOffset = -0.3f;
    [SerializeField] private float verticalOffset = -0.1f;


    [Header("Settings")]
    [SerializeField] private bool hiddenAtStart = true;


    private InputDevice leftController;
    private bool previousButtonState;


    private void Start()
    {
        if (spawnMenu != null && hiddenAtStart)
            spawnMenu.SetActive(false);
    }


    private void Update()
    {
        if (!leftController.isValid)
        {
            leftController = InputDevices.GetDeviceAtXRNode(
                XRNode.LeftHand
            );
        }


        if (!leftController.isValid)
            return;


        if (!leftController.TryGetFeatureValue(
            CommonUsages.secondaryButton,
            out bool buttonPressed))
        {
            return;
        }


        if (buttonPressed && !previousButtonState)
        {
            ToggleMenu();
        }


        previousButtonState = buttonPressed;
    }


    private void ToggleMenu()
    {
        if (spawnMenu == null || playerCamera == null)
        {
            Debug.LogWarning("Spawn menu references missing.");
            return;
        }


        bool shouldOpen = !spawnMenu.activeSelf;


        if (shouldOpen)
        {
            PositionMenu();
        }


        spawnMenu.SetActive(shouldOpen);
    }


    private void PositionMenu()
    {
        Vector3 forward = playerCamera.forward;
        forward.y = 0f;


        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;


        forward.Normalize();


        Vector3 right = playerCamera.right;
        right.y = 0f;
        right.Normalize();


        Vector3 targetPosition =
            playerCamera.position +
            forward * distance +
            right * horizontalOffset +
            Vector3.up * verticalOffset;


        spawnMenu.transform.position = targetPosition;


        // Match the menu's front face to the player's direction.
        spawnMenu.transform.rotation =
            Quaternion.LookRotation(forward, Vector3.up);
    }
}