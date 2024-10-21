using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]private ItemClass item;
    public int Quantity { get; set; }
    public ItemClass Item => item;
}
