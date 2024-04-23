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
        Debug.Log("##### 1 "+arg0.faderTrigger);
        if (arg0.faderTrigger == "crossfade") StartCoroutine(corout_Crossfade());
        else
        {
            Debug.Log("animator aca? "+arg0.faderTrigger);
            animator.SetTrigger(arg0.faderTrigger);
        }
    }


    public void OnEventRaised()
    {
        Debug.Log("##### 2");
        animator.SetTrigger("in");
    }

    IEnumerator corout_Crossfade()
    {
        Debug.Log("crossfade...");
        animator.SetTrigger("out");
        yield return new WaitForSecondsRealtime(1f);
        faderEvent.Raise(new FaderArgs { faderTrigger = "crossfadedone" });
        yield return new WaitForSecondsRealtime(1f);
        animator.SetTrigger("in");
    }

    public void OnEventRaised(SceneArgs arg0)
    {
        Debug.Log("##### 3 "+arg0.sceneAction);
        if (arg0.sceneAction.Contains("loaded")) animator.SetTrigger("in");
        else if (arg0.sceneAction.Contains("ui")) animator.SetTrigger("out");
    }
}


