using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pack of sounds organized by category (SFX, Music, UI, Ambient, etc.).
/// Edit sounds directly in the inspector — no separate ScriptableObject per sound needed.
/// </summary>
[CreateAssetMenu(fileName = "NewSoundPack", menuName = "Audio/Sound Pack")]
public class SoundPack : ScriptableObject
{
    [Header("Pack Info")]
    public string packName;

    [Header("Sounds")]
    public List<Sound> sounds = new List<Sound>();
}
