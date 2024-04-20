using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

[CreateAssetMenu]
public class FaderEvent : GameEvent<FaderArgs>
{
   
}

public class FaderArgs
{
    public string faderTrigger;
}
