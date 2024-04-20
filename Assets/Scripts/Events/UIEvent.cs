using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

[CreateAssetMenu]
public class UIEvent : GameEvent<UIArgs>
{
    
}

public class UIArgs
{
    public string uiAction;
    public int turnOnPanel;
    public int currentPanel;
    public string trigger;
    public string debuglog;
}
