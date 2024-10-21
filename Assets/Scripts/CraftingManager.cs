using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : BaseInventory
{
    public int id;
    [SerializeField] private Button craftingButton;

    private void Awake()
    {
        craftingButton.onClick.AddListener(OnCraftButtonClicked);
    }

    private void OnCraftButtonClicked()
    {
        // Handle crafting
        Craft();
    }

    public void Craft()
    {
        var recipe = craftingRecipes[0];
        if (recipe.CanCraft(this))
        {
            recipe.Craft(this);
        }
        else
        {
            //show error msg
            Debug.Log("Cant craft that item!");
        }
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
