using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupGallery : PopupBase
{
    [SerializeField]
    private Button _BtnBack;
    [SerializeField]
    private Button _BtnCollectCoin;
    [SerializeField]
    private TMP_Text _TxtCoin;
    [SerializeField]
    private TMP_Text _TxtRelics;
    [SerializeField]
    private Transform _Contain;
    [SerializeField]
    private GalleryRelic _PrefRelic;
    [SerializeField]
    private TMP_Text _TxtGalleryName;

    private GalleryData _data;
    private GalleryRelicData[] _galleryRelicDatas;

    private GalleryRelic[] _rels;
    private int _amountCoin = 0;

    public override void OnInit(object[] paras = null)
    {
        base.OnInit(paras);

        int IDGallery = (int) paras[0];
        _data = ResourceManager.Instance.GetGalleryDataByID(IDGallery);
        _galleryRelicDatas = MainPlayer.Instance.GetGalleryRelicByID(_data.ID);

        _amountCoin = 0;
        _rels = InitGalleryRelics();
        foreach (GalleryRelicData galleryRelicData in _galleryRelicDatas)
        {
            _rels[galleryRelicData.Position].OnInit(galleryRelicData, _data.ID, _data.IDRelics);
        }        

        UpdateGalleryName();
        UpdateTxtRelics();
        UpdateTxtCoin();
        UpdateBtnCollectCoin();
    }

    private GalleryRelic[] InitGalleryRelics()
    {
        int numberOfChild = _Contain.childCount;

        if(numberOfChild > _data.IDRelics.Length)
        {
            for(int i = _data.IDRelics.Length; i < numberOfChild; i++)
            {
                _Contain.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if(numberOfChild < _data.IDRelics.Length)
        {
            for (int i = numberOfChild; i < _data.IDRelics.Length; i++)
            {
                Instantiate(_PrefRelic, _Contain);
            }
        }

        List<GalleryRelic> list = new List<GalleryRelic>();
        for(int i = 0; i < _data.IDRelics.Length; i++)
        {
            if(_Contain.GetChild(i).TryGetComponent<GalleryRelic>(out GalleryRelic relic))
            {
                relic.gameObject.SetActive(true);
                relic.OnInit(null, _data.ID, _data.IDRelics);
                list.Add(relic);
            }
        }
        return list.ToArray();
    }

    private void OnEnable()
    {
        GalleryRelic.OnRelicClaim += GalleryRelic_OnRelicClaim;
        GalleryRelic.OnCountdownCompleted += GalleryRelic_OnCoutDownCompleted;
    }

    private void OnDisable()
    {
        GalleryRelic.OnRelicClaim -= GalleryRelic_OnRelicClaim;
        GalleryRelic.OnCountdownCompleted -= GalleryRelic_OnCoutDownCompleted;
    }

    private void Start()
    {
        _BtnBack.onClick.AddListener(OnClickBtnBack);
        _BtnCollectCoin.onClick.AddListener(OnClickBtnCollect);
    }

    private void OnDestroy()
    {
        _BtnBack.onClick.RemoveListener(OnClickBtnBack);
    }

    private void OnClickBtnBack()
    {
        GUIManager.Instance.ShowScreen<ScreenMain>();
        Hide();
    }

    private void OnClickBtnCollect()
    {
        foreach(GalleryRelic rel in _rels)
        {
            rel.OnClaim();
        }
    }

    private void GalleryRelic_OnRelicClaim(int amount)
    {
        _amountCoin -= amount;
        UpdateTxtCoin();
        UpdateBtnCollectCoin();
    }

    private void GalleryRelic_OnCoutDownCompleted(int amount)
    {
        _amountCoin += amount;
        UpdateTxtCoin();
        UpdateBtnCollectCoin();
    }

    private void UpdateTxtCoin()
    {
        if(_amountCoin == 0)
        {
            _TxtCoin.text = "Claim";
        }
        else
        {
            _TxtCoin.text = _amountCoin + " Coin";
        }        
    }

    private void UpdateGalleryName()
    {
        _TxtGalleryName.text = _data.Name;
    }

    private void UpdateTxtRelics()
    {
        if(_galleryRelicDatas.Length == 0)
        {
            _TxtRelics.text = "You don't have any relics \n Collect it !!";
        }
        else
            _TxtRelics.text = $"You have {_galleryRelicDatas.Length}/{_data.IDRelics.Length} Relics";
    }

    private void UpdateBtnCollectCoin()
    {
        if(_amountCoin == 0)
        {
            _BtnCollectCoin.gameObject.SetActive(false);
        }
        else
        {
            _BtnCollectCoin.gameObject.SetActive(true);
        }

    }
}
