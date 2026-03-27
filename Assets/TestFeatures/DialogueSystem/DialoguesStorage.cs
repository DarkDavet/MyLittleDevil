using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueStorageN_", menuName = "Dialogue System/Create dialogue storage")]
public class DialoguesStorage : ScriptableObject
{
    [SerializeField] private List<DialogueSetup> _dialogueStorage;
    public List<DialogueSetup> DialogueSetups { get { return _dialogueStorage; } }
}
