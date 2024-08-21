using Audio_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionRelic : MonoBehaviour
{
    [SerializeField]
    private Toggle _BtnSelection;
    [SerializeField]
    private Image _ImageRelic;

    private Sprite _relicArt;
    private RelicData _data;
    private GalleryRelicData _galleryRelicData;
    private PopupGalleryRelicSelecter _popupSlection;

    //public void OnInit(PopupGalleryRelicSelecter popupSlection, int Id)
    //{
    //    _data = ResourceManager.Instance.GetRelicDataByID(Id);
    //    _relicArt = ResourceManager.Instance.GetRelicSpriteByID(Id);
    //    _ImageRelic.sprite = _relicArt;
    //    _popupSlection = popupSlection;
    //}

    public void OnInit(PopupGalleryRelicSelecter popupSlection, GalleryRelicData galleryRelicData, bool isSelect = false)
    {
        _galleryRelicData = galleryRelicData;
        _data = ResourceManager.Instance.GetRelicDataByID(_galleryRelicData.IDRelic);
        _relicArt = ResourceManager.Instance.GetRelicSpriteByID(_data.ID);
        _ImageRelic.sprite = _relicArt;
        _popupSlection = popupSlection;

        this._BtnSelection.onValueChanged.RemoveAllListeners();
        this._BtnSelection.onValueChanged.AddListener(OnClickSelection);

        if (this._BtnSelection.isOn && isSelect)
            this.OnClickSelection(isSelect);

        this._BtnSelection.isOn = isSelect;
    }    

    private void OnClickSelection(bool isOn)
    {
        if (_galleryRelicData == null || _relicArt == null) return;

        SFX_ClickSelection();
        if (isOn)
            _popupSlection.ShowSelection(_galleryRelicData, _relicArt);
    }

    private void SFX_ClickSelection()
    {
        SoundData soundData = SoundResource.Instance.SelectRelic;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }
}
