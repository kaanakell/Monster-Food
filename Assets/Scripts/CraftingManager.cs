using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : BaseInventory
{

    [SerializeField] private Button craftingButton;

    private void Awake()
    {
        craftingButton.onClick.AddListener(OnCraftButtonClicked);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))//handle crafting
        {
            Craft(craftingRecipes[0]);
        }

        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
        if (Input.GetMouseButtonDown(0))//mouse left click
        {
            //find the closest slot(the slot we clicked on)
            if (isMovingItem)
            {
                //end item move
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }
        else if (Input.GetMouseButtonDown(1))//right click
        {
            //find the closest slot(the slot we clicked on)
            if (isMovingItem)
            {
                //end item move
                EndItemMove_Single();
            }
            else
            {
                BeginItemMove_Half();
            }
        }
    }*/

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
