
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class VRObjectLock : MonoBehaviour
{
    [SerializeField] private bool isLocked;

    private XRGrabInteractable grab;

    public bool IsLocked => isLocked;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (!isLocked)
            return;

        // Prevent grabbing a locked object.
        if (grab.interactionManager != null)
        {
            grab.interactionManager.SelectExit(
                args.interactorObject,
                grab
            );
        }
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;

        Debug.Log(
            $"{gameObject.name}: " +
            (isLocked ? "Locked" : "Unlocked")
        );
    }
}
