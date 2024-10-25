using System.Collections;
using UnityEngine;

[System.Serializable]
public class ItemClass : ScriptableObject
{
    [Header("Item")]//data shared across every item
    public string itemName;
    public int Id;
    public Sprite itemIcon;
    public bool isStackable = true;


    public virtual void Use(PlayerControl caller)
    {
        Debug.Log("Use Item");
    }
    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
}
