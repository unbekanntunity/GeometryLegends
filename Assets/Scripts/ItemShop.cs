using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("ItemSlots")]
    public int X_START_ITEM;
    public int Y_START_ITEM;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN_ITEM;

    [Header("Layout")]
    public int X_START_LAYOUT;
    public int Y_START_LAYOUT;
    public int Y_SPACE_BETWEEN_LAYOUT;

    [SerializeField]
    private GameObject contentPanel, itemPlaceHolder, layoutpanel;

    private int verticalLayoutGroupIndex = 0;
    private List<GameObject> verticalLayoutGroups = new List<GameObject>();

    private void Awake()
    {
        CreateInventory();
    }

    public void CreateInventory()
    {
        for (int i = 0; i < (items.Count / 4) + 1; i++)
        {
            var verticalLayoutGroup = Instantiate(layoutpanel, Vector3.zero, Quaternion.identity);
            verticalLayoutGroup.transform.SetParent(contentPanel.transform);
            verticalLayoutGroup.GetComponent<RectTransform>().localPosition = GetPositionLayout(i);
            verticalLayoutGroups.Add(verticalLayoutGroup);
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (i % 4 == 0 && i != 0)
                verticalLayoutGroupIndex += 1;

            var obj = Instantiate(itemPlaceHolder, Vector3.zero, Quaternion.identity, transform);
            obj.transform.SetParent(verticalLayoutGroups[verticalLayoutGroupIndex].transform);
            obj.GetComponent<RectTransform>().localPosition = GetPositionItem(i);
            obj.GetComponent<ItemSlot>().SetItem(items[i]);
        }
    }

    public Vector3 GetPositionItem(int i)
    {
        return new Vector3(X_START_ITEM + X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN_ITEM), Y_START_ITEM, 0f);
    }

    public Vector3 GetPositionLayout(int i)
    {
        return new Vector3(X_START_LAYOUT, Y_START_LAYOUT - Y_SPACE_BETWEEN_LAYOUT * i, 0f);
    }
}

