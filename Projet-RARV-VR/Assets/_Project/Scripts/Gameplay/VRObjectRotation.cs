using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


[RequireComponent(typeof(XRGrabInteractable))]
public class VRObjectRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationStep = 90f;
    [SerializeField] private float joystickThreshold = 0.7f;


    private XRGrabInteractable grabInteractable;
    private InputDevice rightController;
    private bool joystickReleased = true;
    private Rigidbody rb;


    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.trackRotation = false;
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (!grabInteractable.isSelected)
        {
            joystickReleased = true;
            return;
        }


        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(
                XRNode.RightHand
            );
        }


        if (!rightController.isValid)
            return;


        if (!rightController.TryGetFeatureValue(
                CommonUsages.primary2DAxis,
                out Vector2 joystick))
        {
            return;
        }


        // The joystick must return to the center
        // before another rotation is allowed.
        if (Mathf.Abs(joystick.x) < 0.3f)
        {
            joystickReleased = true;
            return;
        }


        if (!joystickReleased)
            return;


        if (joystick.x >= joystickThreshold)
        {
            RotateObject(1);
            joystickReleased = false;
        }
        else if (joystick.x <= -joystickThreshold)
        {
            RotateObject(-1);
            joystickReleased = false;
        }
    }


    private void RotateObject(int direction)
    {
        float currentY = transform.eulerAngles.y;


        float snappedY =
            Mathf.Round(currentY / rotationStep) * rotationStep;


        float newY = Mathf.Repeat(
            snappedY + direction * rotationStep,
            360f
        );

        Quaternion targetRotation = Quaternion.Euler(0f, newY, 0f);

        rb.rotation = targetRotation;

        Debug.Log($"{gameObject.name}: rotation {newY}°");
    }
}