using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item = null;
    [SerializeField] private int quantity = 0;
    public BaseInventory Inventory {get; set;}

    public SlotClass(BaseInventory baseInventory)
    {
        Inventory = baseInventory;
        item = null;
        quantity = 0;
    }

    public SlotClass(ItemClass _item, int _quantity, BaseInventory baseInventory)
    {
        item = _item;
        quantity = _quantity;
        Inventory = baseInventory;
    }

    public SlotClass(SlotClass slot)
    {
        this.item = slot.GetItem();
        this.quantity = slot.GetQuantity();
        Inventory = slot.Inventory;

    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }

    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void AddQuantity(int _quantity) { quantity += _quantity; }
    public void SubQuantity(int _quantity)
    {
        quantity -= _quantity;
        if(quantity <= 0)
        {
            Clear();
        }
    }
    public void AddItem(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

}
