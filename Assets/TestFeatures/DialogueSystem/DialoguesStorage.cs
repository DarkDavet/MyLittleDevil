using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueStorageN_", menuName = "Dialogue System/Create dialogue storage")]
public class DialoguesStorage : MonoBehaviour
{
    [SerializeField] private List<DialogueSetup> dialogueStorage;
    public List<DialogueSetup> DialogueSetups { get; private set; }
}
