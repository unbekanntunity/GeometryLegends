using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    private ItemSlot[] itemSlot;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        itemSlot = GetComponentsInChildren<ItemSlot>();
        CreateInventory();
    }

    public void CreateInventory()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            try
            {
                itemSlot[i].item = items[i];
                itemSlot[i].name = items[i].itemName;
                itemSlot[i].GetComponentInChildren<Image>().sprite = items[i].image;
            }
            catch (Exception e)
            {
                continue;
            }
        }
    }
}
