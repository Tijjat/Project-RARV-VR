
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRObjectSettingsMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TMP_Text objectName;
    [SerializeField] private Transform parametersContainer;
    [SerializeField] private VRParameterRow parameterRowPrefab;
    [SerializeField] private Button lockButton;
    [SerializeField] private TMP_Text lockButtonText;

    private VRPlaceableObject selectedObject;
    private VRObjectParameters objectParameters;
    private VRObjectLock objectLock;
    public bool IsOpen => gameObject.activeSelf;
    private void Awake()
    {
        lockButton.onClick.AddListener(ToggleLock);
        gameObject.SetActive(false);
    }

    public void Open(VRPlaceableObject target)
    {
        if (target == null)
            return;

        selectedObject = target;
        objectParameters =
            target.GetComponent<VRObjectParameters>();
        objectLock =
            target.GetComponent<VRObjectLock>();

        gameObject.SetActive(true);
        objectName.text = target.gameObject.name;

        foreach (Transform child in parametersContainer)
            Destroy(child.gameObject);

        if (objectParameters != null)
            GenerateParameters();

        UpdateLockButton();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        // Remove generated parameter rows.
        foreach (Transform child in parametersContainer)
        {
            Destroy(child.gameObject);
        }

        selectedObject = null;
        objectParameters = null;
        objectLock = null;
    }


    private void GenerateParameters()
    {
        LevelObjectParameters p = objectParameters.Parameters;

        switch (selectedObject.PrefabId)
        {
            case "platform_moving":
                AddParameter("Distance", p.distance,
                    v => p.distance = v);
                AddParameter("Speed", p.speed,
                    v => p.speed = v);
                AddParameter("Pause at Ends", p.pauseAtEnds,
                    v => p.pauseAtEnds = v);
                break;

            case "trap_bumper":
                AddParameter("Bumper Force", p.bumperForce,
                    v => p.bumperForce = v);
                AddParameter("Cooldown", p.cooldown,
                    v => p.cooldown = v);
                break;

            case "platform_trampoline":
                AddParameter("Trampoline Force", p.trampolineForce,
                    v => p.trampolineForce = v);
                break;

            case "trap_fan":
                AddParameter("Wind Force", p.windForce,
                    v => p.windForce = v);
                AddParameter("Sweep Angle", p.sweepAngle,
                    v => p.sweepAngle = v);
                AddParameter("Sweep Speed", p.sweepSpeed,
                    v => p.sweepSpeed = v);
                AddParameter("Active Duration", p.activeDuration,
                    v => p.activeDuration = v);
                AddParameter("Cooldown Duration", p.cooldownDuration,
                    v => p.cooldownDuration = v);
                break;
        }
    }


    private void AddParameter(
        string label,
        float value,
        Action<float> onChanged)
    {
        VRParameterRow row = Instantiate(
            parameterRowPrefab,
            parametersContainer);

        row.Setup(label, value, onChanged);
    }

    private void ToggleLock()
    {
        if (objectLock == null)
            return;

        objectLock.SetLocked(!objectLock.IsLocked);
        UpdateLockButton();
    }

    private void UpdateLockButton()
    {
        lockButton.interactable = objectLock != null;

        if (objectLock != null)
            lockButtonText.text =
                objectLock.IsLocked ? "Unlock" : "Lock";
    }
}
