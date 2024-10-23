using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : BaseInventory
{
    public int id;
    [SerializeField] private Button craftingButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cookSound;

    private void Awake()
    {
        craftingButton.onClick.AddListener(OnCraftButtonClicked);
    }

    private void OnCraftButtonClicked()
    {
        audioSource.PlayOneShot(cookSound);
        // Handle crafting
        Craft();
    }

    public void Craft()
    {
        foreach (var recipe in craftingRecipes)
        {
            // Check if the current recipe can be crafted
            if (recipe.CanCraft(this))
            {
                recipe.Craft(this);
                Debug.Log("Crafted: " + recipe.outputItem.GetItem().itemName);
                return;  // Exit the method after crafting to avoid crafting multiple recipes at once
            }
        }

        // If none of the recipes can be crafted, show an error message
        Debug.Log("Can't craft any items!");
    }


    public bool AddCraftedItem(ItemClass item, int quantity)
    {
        SlotClass slots = Contains(item);
        if (slots != null && slots.GetItem().isStackable)
        {
            slots.AddQuantity(quantity);
        }
        else
        {

            items[3].AddItem(item, quantity);

        }
        RefreshUI();
        return true;
    }


}
