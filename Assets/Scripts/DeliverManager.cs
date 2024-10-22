using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverManager : BaseInventory
{
    public int[] ids;
    private int currentId;
    [SerializeField] private Button submitButton;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DataArrayObject dataArrayObject;
    private void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        SetCurrentId();
        VisualizeSetCurrentId();

    }

    private void OnSubmitButtonClicked()
    {
        // Handle crafting
        Submit();
    }

    public void Submit()
    {
        var slotItem = items[0].GetItem();
        if (slotItem.Id == currentId)
        {
            //Timer reset
            Debug.Log("Ids match: " + slotItem.Id + " == " + currentId);
            SetCurrentId();
            VisualizeSetCurrentId();
            Remove(items[0].GetItem());
        }
        else
        {
            //GameOver
             Debug.Log("Ids don't match: " + slotItem.Id + " != " + currentId);
        }
    }

    public bool AddSubmitItem(ItemClass item, int quantity)
    {
        SlotClass slots = Contains(item);
        if (slots != null && slots.GetItem().isStackable)
        {
            slots.AddQuantity(quantity);
        }
        else
        {

            items[0].AddItem(item, quantity);

        }
        RefreshUI();
        return true;
    }

    public void SetCurrentId()
    {
        var Random = UnityEngine.Random.Range(0, ids.Length);
        currentId = Random;
    }

    public void VisualizeSetCurrentId()
    {
        spriteRenderer.sprite = dataArrayObject.dataArray[currentId].itemIcon;
    }
}
