using UnityEngine;
using UnityEngine.UI;


public class VRTutorialMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private Button closeButton;


    [Header("Position")]
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float verticalOffset = -0.1f;


    private void Start()
    {
        if (tutorialMenu == null ||
            playerCamera == null ||
            closeButton == null)
        {
            Debug.LogError("Tutorial menu references are missing.");
            return;
        }


        closeButton.onClick.AddListener(CloseTutorial);


        PositionTutorial();
        tutorialMenu.SetActive(true);
    }


    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseTutorial);
    }


    private void PositionTutorial()
    {
        Vector3 forward = playerCamera.forward;
        forward.y = 0f;


        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;


        forward.Normalize();


        tutorialMenu.transform.position =
            playerCamera.position +
            forward * distance +
            Vector3.up * verticalOffset;


        tutorialMenu.transform.rotation =
            Quaternion.LookRotation(forward, Vector3.up);
    }


    private void CloseTutorial()
    {
        tutorialMenu.SetActive(false);
        Debug.Log("Tutorial closed.");
    }
}