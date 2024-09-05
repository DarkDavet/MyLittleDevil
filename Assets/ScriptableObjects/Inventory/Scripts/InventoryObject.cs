using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    //public string savePath;
    //private ItemDatabaseObject database;
    private List<InventorySlot> Container = new List<InventorySlot>();
    public Dictionary<ItemObject, int> dict;
    private bool isItemUsed = false;
    public event Action OnInventoryChanged;

    public int Count { get { return Container.Count; } }
    public InventorySlot this[int index] { get { return Container[index]; } }

    public bool Contains(ItemObject _item)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        Container.Clear();
        OnInventoryChanged?.Invoke();
    }

   /* private void OnEnable()
    {
        
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }*/
    public void AddItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                OnInventoryChanged?.Invoke();
                return;
            }
        }
        Container.Add(new InventorySlot(/*database.GetId[_item], */_item, _amount));
        OnInventoryChanged?.Invoke();
    }

    /*public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, savePath), saveData);
        Debug.Log("Inventory data has saved");

    }
    public void Load()
    {
        string saveFullPath = Path.Combine(Application.persistentDataPath, savePath);
        if (File.Exists(saveFullPath))
        {
            string saveData = File.ReadAllText(saveFullPath);
            JsonUtility.FromJsonOverwrite(saveData, this);
            Debug.Log("Inventory data has loaded");
        }
    }*/

    /*public void OnAfterDeserialize()
    {
        if (database != null)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                Container[i].item = database.GetItem[Container[i].ID];
            }
        }
    }*/

    public void OnBeforeSerialize()
    {

    }

    public void UseItem(ItemObject item)
    {
        Debug.Log($"Using item: {item.name}");
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Container[i].Use();
                if (Container[i].amount <= 0)
                {
                    Container.RemoveAt(i);
                }
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public void OnItemClick(ItemObject item)
    {
        if (!isItemUsed)
        {
            isItemUsed = true;
            UseItem(item);
            CoroutineHelper.Instance.StartHelperCoroutine(ResetItemUseFlag());
        }
    }
    private IEnumerator ResetItemUseFlag()
    {
        yield return new WaitForSeconds(0.1f); // Adjust time as needed
        isItemUsed = false;
    }
}

[System.Serializable]
public class InventorySlot
{
    //public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(/*int _id, */ItemObject _item, int _amount)
    {
        //ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void Use()
    {
        item.Use();
        amount--;
    }
}
