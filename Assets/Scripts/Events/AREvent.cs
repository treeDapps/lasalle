using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

[CreateAssetMenu]
public class AREvent : GameEvent<ARArgs>
{

}

public class ARArgs
{
    public string arAction;
    public Transform spawnedObject;
}
