
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(VRPlaceableObject))]
public class VRObjectSnapping : MonoBehaviour
{
    [SerializeField] private float snapDistance = 0.35f;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private VRSnapPoints[] snapPoints;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        snapPoints = GetComponentsInChildren<VRSnapPoints>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        foreach (VRSnapPoints point in snapPoints)
            point.Disconnect();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        StartCoroutine(SnapAfterRelease());
    }

    private IEnumerator SnapAfterRelease()
    {
        yield return new WaitForEndOfFrame();

        if (grab.isSelected)
            yield break;

        VRSnapPoints bestSource = null;
        VRSnapPoints bestTarget = null;
        float bestDistance = snapDistance;

        VRSnapPoints[] allPoints =
            FindObjectsByType<VRSnapPoints>(
                FindObjectsSortMode.None);

        foreach (VRSnapPoints source in snapPoints)
        {
            if (source.IsOccupied)
                continue;

            foreach (VRSnapPoints target in allPoints)
            {
                if (!source.CanConnect(target))
                    continue;

                float distance = Vector3.Distance(
                    source.transform.position,
                    target.transform.position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestSource = source;
                    bestTarget = target;
                }
            }
        }

        if (bestSource == null)
            yield break;

        // Face the two snap points toward each other.
        Quaternion desiredRotation =
            bestTarget.transform.rotation *
            Quaternion.Euler(0f, 180f, 0f);

        Quaternion rotationDelta =
            desiredRotation *
            Quaternion.Inverse(bestSource.transform.rotation);

        Quaternion newRotation =
            rotationDelta * transform.rotation;

        Vector3 localOffset =
            transform.InverseTransformPoint(
                bestSource.transform.position);

        Vector3 newPosition =
            bestTarget.transform.position -
            newRotation * localOffset;

        VRBuildBounds bounds =
            FindAnyObjectByType<VRBuildBounds>();

        if (rb != null)
        {
            rb.position = newPosition;
            rb.rotation = newRotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            transform.SetPositionAndRotation(
                newPosition, newRotation);
        }
        if (bounds != null)
        {
            bounds.ClampObject(gameObject);

            if (Vector3.Distance(
                bestSource.transform.position,
                bestTarget.transform.position) > 0.01f)
            {
                yield break;
            }
        }

        bestSource.Connect(bestTarget);

        if (bestSource.ConnectedTo == bestTarget)
        {
            VRAudioManager.Instance?.PlaySnap();

            Debug.Log(
            $"{name} snapped to {bestTarget.name}");
        }
    }
}
