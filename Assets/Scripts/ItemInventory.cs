using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public int X_START_ITEM;
    public int Y_START_ITEM;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;

    public GameObject itemSlot;

    public List<Item> purchasedItem = new List<Item>();
    public List<GameObject> itemSlots = new List<GameObject>();

    private void Awake()
    {
        CreateInterface();
    }

    public void CreateInterface()
    {
        for (int i = 0; i < NUMBER_OF_COLUMN; i++)
        {
            var obj = Instantiate(itemSlot, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(gameObject.transform);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(X_START_ITEM + X_SPACE_BETWEEN_ITEM * i, Y_START_ITEM, 0f);
            obj.GetComponent<ItemSlot>().setGoldVisibleState(false);
            itemSlots.Add(obj);
        }
    }

    public void UpdateInterface()
    {
        foreach (GameObject item in itemSlots)
        {
            Destroy(item);
        }

        CreateInterface();

        for(int i = 0; i < purchasedItem.Count; i++)
        {
            itemSlots[i].GetComponent<ItemSlot>().SetItem(purchasedItem[i]);
        }
    }

    public void AddItem(Item item)
    {
        purchasedItem.Add(item);
        UpdateInterface();
    }

    public void RemoveItem(Item item)
    {
        purchasedItem.Remove(item);
        UpdateInterface();
    }
}
