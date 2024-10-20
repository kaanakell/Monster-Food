using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseInventory : MonoBehaviour
{
    [SerializeField] protected List<CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private SlotClass[] startingItems;
    private GameObject[] slots;
    protected SlotClass[] items;

    [SerializeField] private ItemClickHandler itemClickHandler;


    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < items.Length; i++)
        {

            items[i] = new SlotClass(this);
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        //set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        Add(itemToAdd, 1);
        Remove(itemToRemove);
        RefreshUI();
        itemClickHandler.RegisterInventory(this);
    }

    #region MovingStuff

    public SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                var slot = items[i];
                if (slot.Inventory == null)
                {
                    slot.Inventory = this;
                }
                return slot;
            }
        }
        return null;
    }
    #endregion

    public virtual void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public bool Add(ItemClass item, int quantity)
    {

        SlotClass slots = Contains(item);
        if (slots != null && slots.GetItem().isStackable)
        {
            slots.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)//this is an empty slot
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }

        }
        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item, int quantity)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(quantity);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public bool isFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == null)
            {
                return false;
            }
        }
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item)
            {
                return items[i];
            }
        }
        return null;
    }

    public bool Contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && items[i].GetQuantity() >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public void SetCursor(SlotClass movingSlot, bool isMovingItem)
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
    }

}
