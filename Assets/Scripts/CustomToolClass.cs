using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool/CustomTool")]
public class CustomToolClass : ToolClass
{
    public GameObject spawnObject;
    public override void Use(PlayerControl caller)
    {
        base.Use(caller);
        Debug.Log("morphin time");
        Instantiate(spawnObject, caller.transform.position, Quaternion.identity);
    }
}
