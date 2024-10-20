using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClickHandler : MonoBehaviour
{
    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;
    private List<BaseInventory> baseInventories;
    private CraftingManager craftingManager;
    public BaseInventory CurrentInventory
    {
        get
        {
            for (int i = 0; i < baseInventories.Count; i++)
            {
                var slot = baseInventories[i].GetClosestSlot();
                if (slot != null)
                {
                    return baseInventories[i];
                }
            }
            return null;
        }
    }

    private void Awake()
    {
        baseInventories = new List<BaseInventory>();
    }
    public void RegisterInventory(BaseInventory baseInventory)
    {
        baseInventories.Add(baseInventory);
        if (baseInventory as CraftingManager)
        {
            craftingManager = baseInventory as CraftingManager;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))//handle crafting
        {
            craftingManager.Craft();
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
        //SetCursor
        if (movingSlot != null && movingSlot.Inventory != null)
        {
            movingSlot.Inventory.SetCursor(movingSlot, isMovingItem);
        }
    }



    private bool BeginItemMove()
    {
        if (CurrentInventory == null)
        { return false; }
        originalSlot = CurrentInventory.GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        CurrentInventory.RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        if (CurrentInventory == null)
        { return false; }
        originalSlot = CurrentInventory.GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;//there is no item to move
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2), originalSlot.Inventory);
        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2));
        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        CurrentInventory.RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        if (CurrentInventory != null)
        {
            originalSlot = CurrentInventory.GetClosestSlot();
        }

        if (CurrentInventory == null || originalSlot == null)
        {
            movingSlot.Inventory.Add(movingSlot.GetItem(), movingSlot.GetQuantity());
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
                    CurrentInventory.RefreshUI();
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
        originalSlot.Inventory.RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = CurrentInventory.GetClosestSlot();

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
        CurrentInventory.RefreshUI();
        return true;
    }




}
