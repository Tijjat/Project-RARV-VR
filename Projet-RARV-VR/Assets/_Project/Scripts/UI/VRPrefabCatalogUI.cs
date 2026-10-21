using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VRPrefabCatalogUI : MonoBehaviour
{
    [Header("Catalog")]
    [SerializeField] private VRPrefabCatalog catalog;


    [Header("Cards")]
    [SerializeField] private VRPrefabCardUI cardPrefab;
    [SerializeField] private Transform cardsContainer;


    [Header("Category Buttons")]
    [SerializeField] private Button essentialsButton;
    [SerializeField] private Button platformsButton;
    [SerializeField] private Button trapsButton;

    [Header("Object Spawner")]
    [SerializeField] private VRObjectSpawner objectSpawner;

    private readonly List<VRPrefabCardUI> activeCards = new();


    private void Start()
    {
        essentialsButton.onClick.AddListener(
            () => ShowCategory(VRPrefabCategory.Essentials));


        platformsButton.onClick.AddListener(
            () => ShowCategory(VRPrefabCategory.Platforms));


        trapsButton.onClick.AddListener(
            () => ShowCategory(VRPrefabCategory.Traps));


        ShowCategory(VRPrefabCategory.Essentials);
    }


    private void ShowCategory(VRPrefabCategory category)
    {
        foreach (VRPrefabCardUI card in activeCards)
            Destroy(card.gameObject);


        activeCards.Clear();


        foreach (VRPrefabEntry entry in catalog.GetByCategory(category))
        {
            VRPrefabCardUI card =
                Instantiate(cardPrefab, cardsContainer, false);


            card.Setup(entry, OnPrefabSelected);
            activeCards.Add(card);
        }
    }


    private void OnPrefabSelected(VRPrefabEntry entry)
    {
        if (objectSpawner == null)
        {
            Debug.LogError("Object Spawner is not assigned !");
            return;
        }

        objectSpawner.Spawn(entry);
    }
}