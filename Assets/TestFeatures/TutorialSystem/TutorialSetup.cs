using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "tutorial_new", menuName = "Tutorial System/New tutorial")]
public class TutorialSetup : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private List<TutorialSlot> tutorialSlots;
    [SerializeField] private bool _isViewed;
    
    public string Id { get { return id; } }
    public List<TutorialSlot> TutorialSlots { get { return tutorialSlots; } }
    public bool IsViewed { get { return _isViewed; } }
    
    public void MarkAsViewed()
    {
        _isViewed = true;
    }
    
    public void MarkAsUnviewed()
    {
        _isViewed = false;
    }
    
    public void LoadViewedStatus()
    {
        if (!string.IsNullOrEmpty(id))
        {
            _isViewed = PlayerPrefs.GetInt("TutorialViewed_" + id, 0) == 1;
        }
    }
    
    public void SaveViewedStatus()
    {
        if (!string.IsNullOrEmpty(id))
        {
            PlayerPrefs.SetInt("TutorialViewed_" + id, _isViewed ? 1 : 0);
        }
    }
    
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
