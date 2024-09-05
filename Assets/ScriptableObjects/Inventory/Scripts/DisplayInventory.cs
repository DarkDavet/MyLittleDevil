using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<ItemObject, GameObject> itemsDisplayed = new Dictionary<ItemObject, GameObject>();
    Dictionary<GameObject, int> placementMap = new Dictionary<GameObject, int>();
    HashSet<int> occupiedPlaces = new HashSet<int>();

    void Start()
    {
        inventory.OnInventoryChanged += UpdateDisplay;
    }

    void OnDestroy()
    {
        inventory.OnInventoryChanged -= UpdateDisplay;
    }

    public void UpdateDisplay()
    {
        if (this == null) return;

        foreach (var slot in new List<ItemObject>(itemsDisplayed.Keys))
        {
            if (!inventory.Contains(slot))
            {
                ReleaseSlot(slot);
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory[i].item))
            {
                itemsDisplayed[inventory[i].item].GetComponentInChildren<TextMeshProUGUI>().text = inventory[i].amount.ToString("n0");
            }
            else
            {
                CreateDisplay(inventory[i].item, inventory[i].amount);
            }
        }
    }

    public void CreateDisplay(ItemObject item, int amount)
    {
        int i = 0;
        for (; i < NUMBER_OF_COLUMN; i++)
        {
            if (occupiedPlaces.Add(i))
            {
                break;
            }
        }

        var obj = Instantiate(item.prefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = new Vector3(
            X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMN)),
            Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)),
            0f
            );
        obj.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString("n0");
        //ItemObject captured = item;
        obj.GetComponentInChildren<Button>().onClick.AddListener(() => inventory.UseItem(item));

        itemsDisplayed.Add(item, obj);
        placementMap.Add(obj, i);
    }

    private void ReleaseSlot(ItemObject slot)
    {
        occupiedPlaces.Remove(placementMap[itemsDisplayed[slot]]);
        placementMap.Remove(itemsDisplayed[slot]);
        Destroy(itemsDisplayed[slot]);
        itemsDisplayed.Remove(slot);
    }
}
