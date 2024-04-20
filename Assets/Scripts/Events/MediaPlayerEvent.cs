using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AuraXR.EventSystem;
using UnityEngine.UI;

[CreateAssetMenu]
public class MediaPlayerEvent : GameEvent<MediaPlayerArgs>
{
    
}

public class MediaPlayerArgs
{
    public int clip;
    public RawImage rawImage;
    public bool loop;
    public bool fitScreenSize;
}
