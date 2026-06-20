using AchievementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievementDatabase", menuName = "Achievement System/Achievement Database")]
public class AchievementDatabase : ScriptableObject
{
    [Tooltip("All AchievementType ScriptableObjects in the game")]
    public List<AchievementType> allAchievements = new List<AchievementType>();
}
