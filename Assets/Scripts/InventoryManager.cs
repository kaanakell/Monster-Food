using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject hotbarSlotHolder;

    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private SlotClass[] startingItems;
    private SlotClass[] items;

    private GameObject[] slots;
    private GameObject[] hotbarSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
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

        RefreshUI();

        Add(itemToAdd, 1);
        Remove(itemToRemove);
    }

    private void Update()
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);

        }

        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;

        selectedItem = items[selectedSlotIndex + (hotbarSlots.Length * 3)].GetItem();
    }

    private void Craft(CraftingRecipeClass recipe)
    {
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

    #region Moving Stuff
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;//there is no item to move
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2));
        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2));
        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())//they are the same they should stack
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);//a = b
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());//b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());//a = c
                    RefreshUI();
                    return true;
                }
            }
            else//place item as usual
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            return false;//there is no item to move
        }

        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
        {
            return false;
        }

        movingSlot.SubQuantity(1);
        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddQuantity(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
        }

        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }
        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i];
            }
        }
        return null;
    }

    #endregion

    #region Inventory Utils
    public void RefreshUI()
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
        RefreshHotbar();
    }

    public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (hotbarSlots.Length * 3)].GetItem().itemIcon;

                if (items[i + (hotbarSlots.Length * 3)].GetItem().isStackable)
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i + (hotbarSlots.Length * 3)].GetQuantity() + "";
                }
                else
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
    public bool Add(ItemClass item, int quantity)
    {
        //items.Add(item);
        //check if inventory contains item

        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
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
    public void UseSelected()
    {
        items[selectedSlotIndex + (hotbarSlots.Length * 3)].SubQuantity(1);
        RefreshUI();
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
    #endregion

}
