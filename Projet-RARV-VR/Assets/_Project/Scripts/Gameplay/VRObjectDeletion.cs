using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class VRObjectDeletion : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private XRBaseInputInteractor rightInteractor;


    private InputDevice rightController;
    private bool previousButtonState;


    private void Update()
    {
        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(
                XRNode.RightHand
            );
        }


        if (!rightController.isValid)
            return;


        if (!rightController.TryGetFeatureValue(
                CommonUsages.secondaryButton,
                out bool buttonPressed))
        {
            return;
        }


        // Detect the moment B is pressed.
        if (buttonPressed && !previousButtonState)
        {
            DeleteSelectedObject();
        }


        previousButtonState = buttonPressed;
    }


    private void DeleteSelectedObject()
    {
        if (rightInteractor == null)
        {
            Debug.LogWarning("Right Interactor is not assigned.");
            return;
        }


        if (!rightInteractor.hasSelection)
        {
            Debug.Log("No object selected for deletion.");
            return;
        }


        var selected = rightInteractor.interactablesSelected;


        if (selected.Count == 0)
            return;


        Component selectedComponent =
            selected[0] as Component;


        if (selectedComponent == null)
            return;


        VRPlaceableObject placeable =
            selectedComponent.GetComponentInParent<VRPlaceableObject>();


        if (placeable == null)
        {
            Debug.Log("Selected object is not deletable.");
            return;
        }


        Debug.Log(
            $"Deleting object: {placeable.Id} ({placeable.PrefabId})"
        );


        Destroy(placeable.gameObject);
    }
}