using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Item item;
    private TMP_Text goldPrice;
    private Image itemIcon;

    private void Awake()
    {
        itemIcon = GetComponentInChildren<Image>();
        goldPrice = GetComponentInChildren<TMP_Text>();
    }

    public void SetItem(Item _item)
    {
        item = _item;

        itemIcon.sprite = item.image;
        gameObject.name = item.itemName;
        goldPrice.SetText(item.goldprice.ToString("n0"));
    }

    public void setGoldVisibleState(bool state)
    {
        goldPrice.enabled = state;
    }

}
