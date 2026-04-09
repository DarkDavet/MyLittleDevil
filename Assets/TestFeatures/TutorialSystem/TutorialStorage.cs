using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStorageN_", menuName = "Tutorial System/Create tutorial storage")]
public class TutorialStorage : ScriptableObject
{
    [SerializeField] private List<TutorialSetup> _tutorialStorage;
    public List<TutorialSetup> TutorialSetups { get { return _tutorialStorage; } }
}
