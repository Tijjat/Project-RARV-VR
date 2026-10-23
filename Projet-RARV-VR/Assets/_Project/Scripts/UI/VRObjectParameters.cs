
using UnityEngine;

public class VRObjectParameters : MonoBehaviour
{
    [SerializeField]
    private LevelObjectParameters parameters =
        new LevelObjectParameters();

    public LevelObjectParameters Parameters => parameters;
}
