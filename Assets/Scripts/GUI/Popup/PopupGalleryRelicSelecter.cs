using Audio_System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupGalleryRelicSelecter : PopupBase
{    

    [SerializeField]
    private Button _BtnClose;
    [SerializeField]
    private Button _BtnSelection;

    [SerializeField]
    private SelectionRelic _RelicItem;
    [SerializeField]
    private ScrollRect _RelicScroll;
    [SerializeField]
    private Transform _RelicContain;
    [SerializeField]
    private TMP_Text _RelicName;
    [SerializeField]
    private TMP_Text _RelicDescription;
    [SerializeField]
    private TMP_Text _RelicValue;
    [SerializeField]
    private TMP_Text _RelicCost;

    [SerializeField]
    private RelicSelecter _RelicSelecter;
    [SerializeField]

    private GalleryData _GalleryData;

    private GalleryRelicData _selectGalleryRelic;
    private RelicData _selectData;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);

        GalleryRelicData galleryRelicData = (GalleryRelicData)paras[0];
        _GalleryData = ResourceManager.Instance.GetGalleryDataByID(galleryRelicData.IDGallery);

        GalleryRelicData[] galleryRelicCollected = MainPlayer.Instance.GetGalleryRelicByID(_GalleryData.ID);
        List<GalleryRelicData> galleryRelicDatas = new List<GalleryRelicData>();
        for(int i = 0; i < _GalleryData.IDRelics.Length; i++)
        {
            GalleryRelicData data = new GalleryRelicData();
            data.IDGallery = galleryRelicData.IDGallery;
            data.IDRelic = _GalleryData.IDRelics[i];
            data.Position = galleryRelicData.Position; //Same at all
            data.State = GalleryRelicState.NONE;
            data.LastTimer = "";

            bool hasCollected = false;
            for(int j = 0; j < galleryRelicCollected.Length; j++)
            {
                GalleryRelicData dataOld = galleryRelicCollected[j];
                //Relic appeared in Gallery
                if (data.IDRelic == dataOld.IDRelic)
                {
                    data.State = dataOld.State;
                    data.LastTimer = dataOld.LastTimer;
                    hasCollected = true;
                    break;
                }
            }

            if(!hasCollected)
                galleryRelicDatas.Add(data);
        }

        SetUpRelicItems(galleryRelicDatas);
    }

    private void SetUpRelicItems(List<GalleryRelicData> galleryRelicDatas)
    {
        int itemsCount = galleryRelicDatas == null ? 0 : galleryRelicDatas.Count;
        int childCount = this._RelicContain.childCount;
        for (int i = 0; i < itemsCount; i++)
        {
            SelectionRelic gObj = null;
            if (i >= childCount)
            {
                gObj = Instantiate(_RelicItem, _RelicContain);
                childCount += 1;
            }
            else
                gObj = this._RelicContain.GetChild(i).GetComponent<SelectionRelic>();
            gObj.gameObject.SetActive(true);
            gObj.OnInit(this, galleryRelicDatas[i], i == 0);
        }

        for (int i = itemsCount; i < childCount; i++)
        {
            this._RelicContain.GetChild(i).gameObject.SetActive(false);
        }
        this._RelicScroll.verticalNormalizedPosition = 1;

    }

    private void Start()
    {
        _BtnClose.onClick.AddListener(OnClickBtnClose);
        _BtnSelection.onClick.AddListener(OnClickBtnSelection);
    }

    private void OnDestroy()
    {
        _BtnClose.onClick.RemoveListener(OnClickBtnClose);
        _BtnSelection.onClick.RemoveListener(OnClickBtnSelection);
    }

    private void OnClickBtnClose()
    {
        Hide();
    }

    private void OnClickBtnSelection()
    {
        SFX_Selection();
        MainPlayer.Instance.SubMaterial(_selectData.Material);

        _selectGalleryRelic.State = GalleryRelicState.COLLECT;
        MainPlayer.Instance.CollectGalleryRelic(_selectGalleryRelic);
        //GUIManager.Instance.ShowPopup<PopupGallery>(_selectGalleryRelic.IDGallery);        
        Hide();
    }

    private void SFX_Selection()
    {
        SoundData soundData = SoundResource.Instance.ButtonBuy;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }

    public void ShowSelection(GalleryRelicData data, Sprite relicArt)
    {
        _selectGalleryRelic = data;
        RelicData relicData = ResourceManager.Instance.GetRelicDataByID(_selectGalleryRelic.IDRelic);
        if (relicData.Material > MainPlayer.Instance.GetMaterial())
        {
            _BtnSelection.interactable = false;
        }
        else
        {
            _BtnSelection.interactable = true;
        }

        UpdateRelicSelection(relicData, relicArt);
    }    

    private void UpdateRelicSelection(RelicData data, Sprite relicArt)
    {
        _selectData = data;

        _RelicName.text = data.Name;
        _RelicDescription.text = data.Description;
        _RelicValue.text = data.Coin + "/H";
        UpdateRelicCost(data.Material);
        _RelicSelecter.Anim_OnInit(relicArt);        
    }

    private void UpdateRelicCost(int cost)
    {
        _RelicCost.text = "-" + cost;
    }
}
