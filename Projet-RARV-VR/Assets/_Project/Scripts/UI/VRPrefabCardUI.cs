using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VRPrefabCardUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardIcon;


    public void Setup(VRPrefabEntry entry, Action<VRPrefabEntry> onSelected)
    {
        cardName.text = entry.displayName;


        if (entry.previewImage != null)
        {
            cardIcon.sprite = entry.previewImage;
            cardIcon.enabled = true;
        }
        else
        {
            cardIcon.enabled = false;
        }


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelected?.Invoke(entry));
    }
}