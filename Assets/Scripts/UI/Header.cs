using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : UIPanel
{
    public override void OnEventRaised(UIArgs arg0)
    {
       if (arg0.turnOnPanel == PanelIndex)
        {
            var enabled = arg0.trigger == "in" || arg0.uiAction == "back" ? true : false;
            canvas.enabled = enabled;
        }
    }
}
