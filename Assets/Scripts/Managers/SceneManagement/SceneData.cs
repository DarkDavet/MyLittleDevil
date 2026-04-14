using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scenes/Scene Data")]
public class SceneData : ScriptableObject
{
    public string sceneID;  
    public string sceneName; 
    public bool isUnlockedByDefault; // for the first level
    // Stats about collectibles in this level
    public CollectibleSystem.LevelCollectibleStats collectibleStats;
}
