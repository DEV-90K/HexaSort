public class ButtonChallengeReplay : ButtonReplay
{
    public override void OnClickBtnReplay()
    {
        SFX_ClickReplay();
        ChallengeManager.Instance.OnReplay();
    }
}
