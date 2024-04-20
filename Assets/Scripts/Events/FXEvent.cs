using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;

[CreateAssetMenu]
public class FXEvent : GameEvent<FXArgs>
{
}

public class FXArgs
{
    public string type;
    public int fx;
    public AudioClip clip;
    public bool finished;
}
