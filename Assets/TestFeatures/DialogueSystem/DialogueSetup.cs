using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotOwnerVisibilityType { Left, Right, None }

[CreateAssetMenu(fileName = "dlg_new", menuName = "Dialogue System/New dialogue")]
public class DialogueSetup : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private List<DialogueSlot> dialogueSlots;

    public string Id { get { return id; } }
    public List<DialogueSlot> DialogueSlots {  get { return dialogueSlots; } }

}

[System.Serializable]
public class DialogueSlot
{
    public SlotOwnerVisibilityType visibility = SlotOwnerVisibilityType.Left;
    public string title;
    public Color title_color;
    public Sprite icon;
    [TextArea(3, 10)]
    public string string_text;
}

