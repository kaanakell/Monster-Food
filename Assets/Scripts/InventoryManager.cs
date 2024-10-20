using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : BaseInventory
{
    [SerializeField] private GameObject hotbarSlotHolder;
    private GameObject[] hotbarSlots;

    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;


    private void Awake()
    {
        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
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

    public override void RefreshUI()
    {
        base.RefreshUI();
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
    
    public void UseSelected()
    {
        items[selectedSlotIndex + (hotbarSlots.Length * 3)].SubQuantity(1);
        RefreshUI();
    }

}
