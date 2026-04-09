using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "tutorial_new", menuName = "Tutorial System/New tutorial")]
public class TutorialSetup : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private List<TutorialSlot> tutorialSlots;

    public string Id { get { return id; } }
    public List<TutorialSlot> TutorialSlots { get { return tutorialSlots; } }

}

[System.Serializable]
public class TutorialSlot
{
    [TextArea(3, 10)]
    public string title;
    [TextArea(3, 10)]
    public string content;
    public Sprite backgroundImage;
    public bool showNextButton = true;
}
