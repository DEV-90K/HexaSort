using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupChallengeWoned : PopupBase
{
    [SerializeField]
    private Button _BtnHome;
    [SerializeField]
    private Button _BtnGallery;
    [SerializeField]
    private Image _ArtRelic;

    private GalleryRelicData _GalleryRelicData;
    public void OnInit()
    {
        _GalleryRelicData = null;

        UpdateArtRelic();
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    private void UpdateArtRelic()
    {
        Sprite art = ResourceManager.Instance.GetRelicSpriteByID(_GalleryRelicData.IDRelic);
        _ArtRelic.sprite = art;
    }

    private void Awake()
    {
        _BtnHome.onClick.AddListener(OnClickBtnHome);
        _BtnGallery.onClick.AddListener(OnClickBtnGallery);
    }

    private void OnDestroy()
    {
        _BtnHome.onClick.RemoveListener(OnClickBtnHome);
        _BtnGallery.onClick.RemoveListener(OnClickBtnGallery);
    }

    private void OnClickBtnHome()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        GUIManager.Instance.HideScreen<ScreenChallenge>();
        GUIManager.Instance.ShowScreen<ScreenMain>();
        PopupManager.Instance.HidePopup<PopupChallengeWoned>();
    }

    private void OnClickBtnGallery()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        GUIManager.Instance.HideScreen<ScreenChallenge>();
        PopupGallery popup = PopupManager.Instance.ShowPopup<PopupGallery>();
        popup.OnInit(_GalleryRelicData.IDGallery);
        PopupManager.Instance.HidePopup<PopupChallengeWoned>();
    }
}
