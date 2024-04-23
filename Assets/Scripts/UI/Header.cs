using AuraXR.EventSystem;
using UnityEngine;

public class Header : UIPanel, IEventListener<ARArgs>
{
    [SerializeField] AREvent arEvent;
    [SerializeField] GameObject backButton;
    public override void OnEventRaised(UIArgs arg0)
    {
       if (arg0.turnOnPanel == PanelIndex)
        {
            var enabled = arg0.trigger == "in" || arg0.uiAction == "back" ? true : false;
            canvas.enabled = enabled;
        }
    }
    void OnEnable()
    {
        arEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        arEvent.UnregisterListener(this);
    }

    public void OnEventRaised(ARArgs data)
    {
        if (data.arAction.Equals("instantiateAssetBundle"))
        {
            backButton.SetActive(true);
        }
    }
}
