using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class PopupRelicSelecter : PopupBase
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
    private Image _RelicArt;

    private GalleryRelic _Grallery;

    private int _idGallery;
    private int[] _idRelics; 
    private int _idxGalleryRelic;

    private RelicData _selectData;
    private Sprite _selectSprite;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        
        _idGallery = (int)paras[0];
        _idRelics = (int[])paras[1];
        _idxGalleryRelic = (int)paras[2];
        _Grallery = (GalleryRelic)paras[3];

        SetUpRelicItems();
        ShowSelection(_idRelics[0]);
    }

    private void SetUpRelicItems()
    {
        if(_RelicContain.childCount > _idRelics.Length)
        {
            for(int i = _idRelics.Length; i < _RelicContain.childCount; i++)
            {
                _RelicContain.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(_RelicContain.childCount < _idRelics.Length)
        {
            for(int i = _RelicContain.childCount; i < _idRelics.Length; i++)
            {
                Instantiate(_RelicItem, _RelicContain);
            }
        }

        for(int i = 0; i < _idRelics.Length; i++)
        {
            if(_RelicContain.GetChild(i).TryGetComponent(out SelectionRelic relic))
            {
                relic.OnInit(this, _idRelics[i]);
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
        //_Grallery.ShowRelic(_idxGalleryRelic, _selectData.ID);

        ChallengeManager.Instance.OnInit(new GalleryRelicData(_idGallery, _selectData.ID, _idxGalleryRelic, DateTime.Now.ToString(), GalleryRelicState.LOCK));
        Hide();
    }

    public void ShowSelection(RelicData data, Sprite relicArt)
    {
        UpdateRelicSelection(data, relicArt);
    }

    private void ShowSelection(int ID)
    {
        RelicData data = ResourceManager.Instance.GetRelicDataByID(ID);
        Sprite relicArt = ResourceManager.Instance.GetRelicSpriteByID(ID);
        ShowSelection(data, relicArt);
    }

    private void UpdateRelicSelection(RelicData data, Sprite relicArt)
    {
        _selectData = data;

        _RelicName.text = data.Name;
        _RelicDescription.text = "+ Description: " + data.Description;
        _RelicValue.text = "+ Display value: " + data.Coin + "Coin" + "/" + data.Timer + "Minute";
        _RelicArt.sprite = relicArt;
    }
}
