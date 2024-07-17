using System.Collections;
using System.Collections.Generic;
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
    private Button _BtnWaiting;

    [SerializeField]
    private Transform _Contain;
    [SerializeField]
    private GalleryRelic _PrefRelic;

    private GalleryData _data;
    private GalleryRelicData[] _galleryRelicDatas;

    private GalleryRelic[] _rels;
    private int _amountCoin = 0;

    public override void OnInit(object[] paras = null)
    {
        base.OnInit(paras);
        _data = ResourceManager.Instance.GetGalleryData();
        _galleryRelicDatas = MainPlayer.Instance.GetGalleryRelicByID(_data.ID);

        _amountCoin = 0;
        _BtnWaiting.gameObject.SetActive(true);
        _BtnCollectCoin.gameObject.SetActive(false);

        _rels = InitGalleryRelics();
        foreach (GalleryRelicData galleryRelicData in _galleryRelicDatas)
        {
            _rels[galleryRelicData.Position].OnInit(galleryRelicData, _data.ID, _data.IDRelics);
        }
    }

    private GalleryRelic[] InitGalleryRelics()
    {
        int numberOfChild = _Contain.childCount;

        if(numberOfChild > _data.Capacity)
        {
            for(int i = _data.Capacity; i < numberOfChild; i++)
            {
                _Contain.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if(numberOfChild < _data.Capacity)
        {
            for (int i = numberOfChild; i < _data.Capacity; i++)
            {
                Instantiate(_PrefRelic, _Contain);
            }
        }

        List<GalleryRelic> list = new List<GalleryRelic>();
        for(int i = 0; i < _data.Capacity; i++)
        {
            if(_Contain.GetChild(i).TryGetComponent<GalleryRelic>(out GalleryRelic relic))
            {
                relic.gameObject.SetActive(true);
                //relic.OnInit(_data.GalleryRelics[i], _data.ID, _data.IDRelics);
                relic.OnInit(null, _data.ID, _data.IDRelics);
                list.Add(relic);
            }
        }
        return list.ToArray();
    }

    private void OnEnable()
    {
        GalleryRelic.OnRelicClaim += GalleryRelic_OnRelicClaim;
        GalleryRelic.OnCoutDownCompleted += GalleryRelic_OnCoutDownCompleted;
    }

    private void OnDisable()
    {
        GalleryRelic.OnRelicClaim -= GalleryRelic_OnRelicClaim;
        GalleryRelic.OnCoutDownCompleted -= GalleryRelic_OnCoutDownCompleted;
    }

    private void Start()
    {
        _BtnBack.onClick.AddListener(OnClickBtnBack);
        _BtnCollectCoin.onClick.AddListener(OnClickBtnCollect);
        _BtnWaiting.onClick.AddListener(OnClickBtnWaiting);
    }

    private void OnDestroy()
    {
        _BtnBack.onClick.RemoveListener(OnClickBtnBack);
    }

    private void OnClickBtnBack()
    {
        Hide();
    }

    private void OnClickBtnCollect()
    {
        foreach(GalleryRelic rel in _rels)
        {
            rel.OnClaim();
        }
    }

    private void OnClickBtnWaiting()
    {
        Debug.Log("OnClickBtnWaiting");
    }
    private void GalleryRelic_OnRelicClaim(int amount)
    {
        _amountCoin -= amount;
        UpdateTxtCoin();

        if (_amountCoin <= 0)
        {
            _BtnCollectCoin.gameObject.SetActive(false);
            _BtnWaiting.gameObject.SetActive(true);
        }
    }

    private void GalleryRelic_OnCoutDownCompleted(int amount)
    {
        _amountCoin += amount;
        UpdateTxtCoin();

        if (_amountCoin > 0)
        {
            _BtnCollectCoin.gameObject.SetActive(true);
            _BtnWaiting.gameObject.SetActive(false);
        }
    }

    private void UpdateTxtCoin()
    {
        _TxtCoin.text = _amountCoin.ToString();
    }
}
