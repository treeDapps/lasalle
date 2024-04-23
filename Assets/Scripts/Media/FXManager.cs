using System.Collections;
using System.Collections.Generic;
using AuraXR.EventSystem;
using UnityEngine;

public class FXManager : MonoBehaviour, IEventListener<FXArgs>
{
    [SerializeField] FXEvent audioclipEvent;

    [SerializeField] AudioClip[] fxs;
    AudioSource player;

    void Awake()
    {
        player = GetComponent<AudioSource>();
    }

    void OnEnable() => audioclipEvent.RegisterListener(this);
    void OnDisable() => audioclipEvent.UnregisterListener(this);
    void OnDestroy() => audioclipEvent.UnregisterListener(this);

    public void PlaySound(int fx)
    {
        player.Stop();
        player.clip = fxs[fx];
        player.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        player.Stop();
        player.clip = clip;
        player.Play();
        StartCoroutine(corout_AudioHasFinished());
    }

    public void OnEventRaised(FXArgs arg0)
    {
        if(arg0.type == "fx") PlaySound(arg0.fx);
        else if(arg0.type == "audioclip") PlayClip(arg0.clip);
    }

    private IEnumerator corout_AudioHasFinished()
    {
        Debug.Log("play sfx "+ player.clip.length);
        yield return new WaitForSeconds(player.clip.length);
        Debug.Log("finished ");
        audioclipEvent.Raise(new FXArgs { finished = true});
    }
}
