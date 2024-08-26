using UnityEngine;
using UnityEngine.UI;

public class PopupChallengeLosed : PopupBase
{
    [SerializeField]
    private Button _BtnHome;
    [SerializeField]
    private Button _BtnReplay;
    [SerializeField]
    private Image _ArtRelic;

    private GalleryRelicData _GalleryRelicData;
    public void OnInit()
    {
        _GalleryRelicData = null;

        UpdateArtRelic();
    }

    private void UpdateArtRelic()
    {
        Sprite art = ResourceManager.Instance.GetRelicSpriteByID(_GalleryRelicData.IDRelic);
        _ArtRelic.sprite = art;
    }

    private void Start()
    {
        _BtnHome.onClick.AddListener(OnClickBtnHome);
        _BtnReplay.onClick.AddListener(OnClickBtnReplay);
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    private void OnDestroy()
    {
        _BtnHome.onClick.RemoveListener(OnClickBtnHome);
        _BtnReplay.onClick.RemoveListener(OnClickBtnReplay);
    }

    private void OnClickBtnHome()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        GUIManager.Instance.HideScreen<ScreenChallenge>();
        GUIManager.Instance.ShowScreen<ScreenMain>();
        PopupManager.Instance.HidePopup<PopupChallengeLosed>();
    }

    private void OnClickBtnReplay()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        ChallengeManager.Instance.OnInit(_GalleryRelicData);
        PopupManager.Instance.HidePopup<PopupChallengeLosed>();
    }
}
