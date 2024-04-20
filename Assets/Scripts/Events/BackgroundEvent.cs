using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

[CreateAssetMenu]
public class BackgroundEvent : GameEvent<BackgroundArgs>
{
    
}

public class BackgroundArgs
{
    public string actionToPerform;
}
