using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


[RequireComponent(typeof(XRGrabInteractable))]
public class VRBuildBoundsObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private VRBuildBounds buildBounds;
    private Coroutine correctionRoutine;


    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        buildBounds = FindAnyObjectByType<VRBuildBounds>();
    }


    private void OnEnable()
    {
        grabInteractable.selectExited.AddListener(OnReleased);
    }


    private void OnDisable()
    {
        grabInteractable.selectExited.RemoveListener(OnReleased);


        if (correctionRoutine != null)
        {
            StopCoroutine(correctionRoutine);
            correctionRoutine = null;
        }
    }


    private void OnReleased(SelectExitEventArgs args)
    {
        if (buildBounds == null)
            return;


        if (correctionRoutine != null)
            StopCoroutine(correctionRoutine);


        correctionRoutine = StartCoroutine(
            CorrectAfterRelease()
        );
    }


    private IEnumerator CorrectAfterRelease()
    {
        // Wait until XR Interaction Toolkit finishes
        // processing the release.
        yield return new WaitForEndOfFrame();


        if (grabInteractable != null &&
            !grabInteractable.isSelected &&
            buildBounds != null)
        {
            buildBounds.ClampObject(gameObject);
        }


        correctionRoutine = null;
    }
}