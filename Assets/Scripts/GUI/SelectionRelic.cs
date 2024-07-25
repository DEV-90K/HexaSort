using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionRelic : MonoBehaviour
{
    [SerializeField]
    private Button _BtnSelection;
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

    public void OnInit(PopupGalleryRelicSelecter popupSlection, GalleryRelicData galleryRelicData)
    {
        _galleryRelicData = galleryRelicData;
        _data = ResourceManager.Instance.GetRelicDataByID(_galleryRelicData.IDRelic);
        _relicArt = ResourceManager.Instance.GetRelicSpriteByID(_data.ID);
        _ImageRelic.sprite = _relicArt;
        _popupSlection = popupSlection;
    }    

    private void Start()
    {
        _BtnSelection.onClick.AddListener(OnClickSelection);
    }

    private void OnDestroy()
    {
        _BtnSelection.onClick.AddListener(OnClickSelection);
    }

    private void OnClickSelection()
    {
        _popupSlection.ShowSelection(_galleryRelicData, _relicArt);
    }
}
