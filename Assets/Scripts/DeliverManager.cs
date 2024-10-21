using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverManager : BaseInventory
{
    public int id;
    [SerializeField] private Button submitButton;
    private void Awake()
    {
        submitButton.onClick.AddListener(OnCraftButtonClicked);
    }

    private void OnCraftButtonClicked()
    {
        // Handle crafting
        Submit();
    }

    public void Submit()
    {
        /*var recipe = craftingRecipes[0];
        if (recipe.CanCraft(this))
        {
            recipe.Craft(this);
        }
        else
        {
            //show error msg
            Debug.Log("Cant craft that item!");
        }*/
    }

    public bool AddSubmitItem(ItemClass item, int quantity)
    {
        SlotClass slots = Contains(item);
        if (slots != null && slots.GetItem().isStackable)
        {
            slots.AddQuantity(quantity);
        }
        else
        {

            items[0].AddItem(item, quantity);

        }
        RefreshUI();
        return true;
    }
}
