using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonChallengeExit : ButtonExit
{
    public override void OnClickExit()
    {
        SFX_ClickExit();
        ChallengeManager.Instance.OnExit();
        GUIManager.Instance.HideScreen<ScreenChallenge>();
    }
}
