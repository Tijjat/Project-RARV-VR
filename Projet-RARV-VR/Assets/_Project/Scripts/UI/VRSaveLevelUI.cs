using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VRSaveLevelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VRLevelFileManager fileManager;


    [Header("Panels")]
    [SerializeField] private GameObject spawnPanel;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject overwritePanel;


    [Header("Level Name")]
    [SerializeField] private TMP_InputField levelNameInput;
    [SerializeField] private TMP_Text saveStatus;


    [Header("Buttons")]
    [SerializeField] private Button openSaveButton;
    [SerializeField] private Button confirmSaveButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button overwriteButton;
    [SerializeField] private Button cancelOverwriteButton;


    private string pendingFileName;


    private void Start()
    {
        openSaveButton.onClick.AddListener(OpenSavePanel);
        confirmSaveButton.onClick.AddListener(TrySave);
        backButton.onClick.AddListener(CloseSavePanel);
        overwriteButton.onClick.AddListener(ConfirmOverwrite);
        cancelOverwriteButton.onClick.AddListener(CancelOverwrite);


        savePanel.SetActive(false);
        overwritePanel.SetActive(false);
    }


    private void OnDestroy()
    {
        openSaveButton.onClick.RemoveListener(OpenSavePanel);
        confirmSaveButton.onClick.RemoveListener(TrySave);
        backButton.onClick.RemoveListener(CloseSavePanel);
        overwriteButton.onClick.RemoveListener(ConfirmOverwrite);
        cancelOverwriteButton.onClick.RemoveListener(CancelOverwrite);
    }


    private void OpenSavePanel()
    {
        saveStatus.text = "";
        pendingFileName = null;


        overwritePanel.SetActive(false);
        spawnPanel.SetActive(false);
        savePanel.SetActive(true);
    }


    private void CloseSavePanel()
    {
        overwritePanel.SetActive(false);
        savePanel.SetActive(false);
        spawnPanel.SetActive(true);
    }


    private void TrySave()
    {
        if (fileManager == null)
        {
            SetStatus("File manager is missing.");
            return;
        }


        string name = levelNameInput.text.Trim();


        if (string.IsNullOrWhiteSpace(name))
        {
            SetStatus("Please enter a level name.");
            return;
        }


        if (fileManager.FileExists(name))
        {
            pendingFileName = name;
            overwritePanel.SetActive(true);
            return;
        }


        Save(name, false);
    }


    private void ConfirmOverwrite()
    {
        if (string.IsNullOrWhiteSpace(pendingFileName))
            return;


        Save(pendingFileName, true);
    }


    private void CancelOverwrite()
    {
        pendingFileName = null;
        overwritePanel.SetActive(false);
        SetStatus("Overwrite cancelled.");
    }


    private void Save(string name, bool overwrite)
    {
        bool success = fileManager.SaveLevel(name, overwrite);


        overwritePanel.SetActive(false);
        pendingFileName = null;


        if (success)
            SetStatus("Level saved successfully!");
        else
            SetStatus("Save failed. Check Unity Console.");
    }


    private void SetStatus(string message)
    {
        if (saveStatus != null)
            saveStatus.text = message;


        Debug.Log(message);
    }
}