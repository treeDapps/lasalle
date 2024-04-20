using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

[CreateAssetMenu]
public class EventScreenshot : GameEvent<ScreenshotArgs>
{
    
}

public class ScreenshotArgs
{
    public string screenshotAction;
}
