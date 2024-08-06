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
    private Transform _RelicContain;
    [SerializeField]
    private TMP_Text _RelicName;
    [SerializeField]
    private TMP_Text _RelicDescription;
    [SerializeField]
    private TMP_Text _RelicValue;

    [SerializeField]
    private TMP_Text _MatAmount;

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
        ShowSelection(galleryRelicDatas[0]);
        UpdateTxtPlayerMaterial();
    }

    private void SetUpRelicItems(List<GalleryRelicData> galleryRelicDatas)
    {
        if (_RelicContain.childCount > galleryRelicDatas.Count)
        {
            for (int i = galleryRelicDatas.Count; i < _RelicContain.childCount; i++)
            {
                _RelicContain.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (_RelicContain.childCount < galleryRelicDatas.Count)
        {
            for (int i = _RelicContain.childCount; i < galleryRelicDatas.Count; i++)
            {
                Instantiate(_RelicItem, _RelicContain);
            }
        }

        for (int i = 0; i < galleryRelicDatas.Count; i++)
        {
            if (_RelicContain.GetChild(i).TryGetComponent(out SelectionRelic relic))
            {
                relic.OnInit(this, galleryRelicDatas[i]);
            }
        }
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
        MainPlayer.Instance.SubMaterial(_selectData.Material);
        _selectGalleryRelic.State = GalleryRelicState.COLLECT;
        MainPlayer.Instance.CollectGalleryRelic(_selectGalleryRelic);
        GUIManager.Instance.ShowPopup<PopupGallery>(_selectGalleryRelic.IDGallery);
        Hide();
    }

    public void ShowSelection(RelicData data, Sprite relicArt)
    {
        if(data.Material > MainPlayer.Instance.GetMaterial())
        {
            _BtnSelection.interactable = false;
        }
        else
        {
            _BtnSelection.interactable = true;
        }

        UpdateRelicSelection(data, relicArt);
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

    private void ShowSelection(GalleryRelicData data)
    {
        _selectGalleryRelic = data;
        RelicData relicData = ResourceManager.Instance.GetRelicDataByID(_selectGalleryRelic.IDRelic);
        Sprite relicArt = ResourceManager.Instance.GetRelicSpriteByID(_selectGalleryRelic.IDRelic);
        ShowSelection(relicData, relicArt);
    }

    private void UpdateRelicSelection(RelicData data, Sprite relicArt)
    {
        _selectData = data;

        _RelicName.text = data.Name;
        _RelicDescription.text = "+ Description: " + data.Description;
        _RelicValue.text = "+ Display value: " + data.Coin + "Coin" + "/" + data.Timer + "Minute";

        _RelicSelecter.Anim_OnInit(relicArt);        
    }


    private void UpdateTxtPlayerMaterial()
    {
        _MatAmount.text = MainPlayer.Instance.GetMaterial().ToString();
    }
}
