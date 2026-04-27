using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Customization/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<CustomizationItem> allItems;

    // Быстрый поиск предмета по ID
    public CustomizationItem GetItemById(string id)
    {
        return allItems.Find(item => item.id == id);
    }

    // Автоматический поиск всех ассетов в проекте (работает только в редакторе)
    [ContextMenu("Find All Items")]
    private void FindAllItems()
    {
#if UNITY_EDITOR
        allItems.Clear();
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:CustomizationItem");
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            allItems.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<CustomizationItem>(path));
        }
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"Найдено и добавлено предметов: {allItems.Count}");
#endif
    }
}
