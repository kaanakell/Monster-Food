using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Consumable")]

public class ConsumableClass : ItemClass
{
    [Header("Consumable")]//data specific to consumable class
    public float healthAdded;

    public override void Use(PlayerControl caller)
    {
        base.Use(caller);
        Debug.Log("Eat Consumable");
        caller.inventory.UseSelected();
    }
    public override ConsumableClass GetConsumable() {return this;}
}
