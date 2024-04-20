using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

[CreateAssetMenu]
public class SceneEvent : GameEvent<SceneArgs>
{
    
}

public class SceneArgs
{
    public string sceneAction;
    public int sceneIndex;
    public string sceneName;
}
