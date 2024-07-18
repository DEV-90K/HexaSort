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
    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _GalleryRelicData = (GalleryRelicData)paras[0];

        UpdateArtRelic();
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
        GUIManager.Instance.ShowScreen<ScreenMain>();
        Hide();
    }

    private void OnClickBtnGallery()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(_GalleryRelicData.IDGallery);
        Hide();
    }
}
