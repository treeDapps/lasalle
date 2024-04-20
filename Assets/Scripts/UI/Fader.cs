using UnityEngine;
using AuraXR.EventSystem;
using System.Collections;

public class Fader : MonoBehaviour, IEventListener<FaderArgs>
                                  , IEventListener<SceneArgs>
{
    [SerializeField] FaderEvent faderEvent;
    [SerializeField] SceneEvent sceneEvent;

    [SerializeField] Animator animator;

    
    void OnEnable()
    {
        faderEvent.RegisterListener(this);
        sceneEvent.RegisterListener(this);
    }
    void OnDisable()
    {
        faderEvent.UnregisterListener(this);
        sceneEvent.UnregisterListener(this);
    }
    void OnDestroy()
    {
        faderEvent.UnregisterListener(this);
        sceneEvent.UnregisterListener(this);
    }

    public void OnEventRaised(FaderArgs arg0)
    {
        if (arg0.faderTrigger == "crossfade") StartCoroutine(corout_Crossfade());
        else animator.SetTrigger(arg0.faderTrigger);
    }


    public void OnEventRaised()
    {
        animator.SetTrigger("in");
    }

    IEnumerator corout_Crossfade()
    {
        animator.SetTrigger("out");
        yield return new WaitForSecondsRealtime(1f);
        animator.SetTrigger("in");
        faderEvent.Raise(new FaderArgs { faderTrigger = "crossfadedone" });
    }

    public void OnEventRaised(SceneArgs arg0)
    {
        if(arg0.sceneAction.Contains("loaded")) animator.SetTrigger("in");
        else if (arg0.sceneAction.Contains("ui")) animator.SetTrigger("out");
    }
}


