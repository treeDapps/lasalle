using System.Collections;
using AuraXR.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class Secure : MonoBehaviour, IEventListener
{
    [SerializeField] BasicGameEvent buttonEvent;
    EventSystem eventSystem;

    void Awake() => eventSystem = GetComponent<EventSystem>();
    private void OnEnable()
    {
        buttonEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        buttonEvent.UnregisterListener(this);
    }

    private void OnDestroy()
    {
        buttonEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        StartCoroutine(corout_Secure());
    }

    public void StartSecure()
    {
        StartCoroutine(corout_Secure());
    }

    IEnumerator corout_Secure()
    {
        eventSystem.enabled = false;
        yield return new WaitForSeconds(1);
        eventSystem.enabled = true;
    }

}
