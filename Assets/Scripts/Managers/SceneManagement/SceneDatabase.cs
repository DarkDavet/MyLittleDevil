using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneDatabase", menuName = "Scenes/Scene Database")]
public class SceneDatabase: ScriptableObject
{
    public List<SceneData> sceneDatabase;
}
