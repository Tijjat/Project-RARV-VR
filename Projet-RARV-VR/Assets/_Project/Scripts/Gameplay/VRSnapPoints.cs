
using UnityEngine;

public class VRSnapPoints : MonoBehaviour
{
    [SerializeField] private string snapType = "Default";

    public string SnapType => snapType;
    public VRSnapPoints ConnectedTo { get; private set; }
    public bool IsOccupied => ConnectedTo != null;

    public bool CanConnect(VRSnapPoints other)
    {
        return other != null
            && other != this
            && !IsOccupied
            && !other.IsOccupied
            && snapType == other.snapType
            && GetComponentInParent<VRPlaceableObject>()
               != other.GetComponentInParent<VRPlaceableObject>();
    }

    public void Connect(VRSnapPoints other)
    {
        if (!CanConnect(other))
            return;

        ConnectedTo = other;
        other.ConnectedTo = this;
    }

    public void Disconnect()
    {
        if (ConnectedTo == null)
            return;

        VRSnapPoints other = ConnectedTo;
        ConnectedTo = null;

        if (other.ConnectedTo == this)
            other.ConnectedTo = null;
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.08f);
        Gizmos.DrawRay(transform.position, transform.forward * 0.25f);
    }
}
