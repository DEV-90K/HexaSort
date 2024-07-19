using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChallengeReplay : ButtonReplay
{
    public override void OnClickBtnReplay()
    {
        ChallengeManager.Instance.OnReplay();
    }
}
