using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]//data specific to tool class
    public ToolType toolType;
    public enum ToolType
    {
        weapon,
        pickaxe,
        hammer,
        axe
    }

    public override void Use(PlayerControl caller)
    {
        base.Use(caller);
        Debug.Log("Swing Tool");
    }

    public override ToolClass GetTool() {return this;}
}
