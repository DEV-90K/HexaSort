using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryRelic : MonoBehaviour
{
    public static Action<int> OnRelicCollection;

    [Header("Model Selecter")]
    [SerializeField]
    private GameObject _ObjSelecter;
    [SerializeField]
    private Button _BtnSelecter;

    [Header("Model Collect")]
    [SerializeField]
    private GameObject _ObjCollect;
    [SerializeField]
    private Button _BtnCollect;
    [SerializeField]
    private Image _ImgRelic;
    [SerializeField]
    private TMP_Text _txtCoinCollect;
    [SerializeField]
    private TMP_Text _txtCountdownCollect;
    [SerializeField]
    private TMP_Text _txtName;

    private GalleryRelicData _data;
    private RelicData _relicData;

    private int _idx;
    public int[] _idRelics;
    private int _idGallery;

    private TimerUtils.CountdownTimer _timer = null;
    public void OnInit(GalleryRelicData galleryRelicData, int idGallery, int[] idRelics)
    {
        _idx = transform.GetSiblingIndex();
        _idGallery = idGallery;
        _idRelics = idRelics;

        _data = galleryRelicData;

        if (_data == null)
        {
            SetUpModelSelecter();
        }
        else
        {
            SetUpModelCollection();
        }
    }

    public void ShowRelic(int idxGalleryRelic, int idRelic)
    {
        GalleryRelicData galleryRelicData = new GalleryRelicData();
        galleryRelicData.IDRelic = idRelic;
        galleryRelicData.LastTimer = DateTime.Now.ToString();

        _data = galleryRelicData;

        if (_data == null)
        {
            SetUpModelSelecter();
        }
        else
        {
            SetUpModelCollection();
        }
    }

    private void SetUpModelSelecter()
    {
        _ObjCollect.SetActive(false);
        _ObjSelecter.SetActive(true);
    }

    private void SetUpModelCollection()
    {
        _ObjSelecter.SetActive(false);
        _ObjCollect.SetActive(true);

        _relicData = ResourceManager.Instance.GetRelicDataByID(_data.IDRelic);
        UpdateTxtName();
        UpdateArtRelic();
        UpdateTxtCoinCollect();
        UpdateTxtCountdownCollect();
    }

    private void UpdateTxtCoinCollect()
    {
        _txtCoinCollect.text = "+" + _relicData.Coin;
    }

    private void UpdateTxtCountdownCollect()
    {
        //DateTime targetTime = _data.LastTimeSpinFreeClicked.AddMinutes(timeToFree).AddSeconds(60); //Countdown => need add 60
        DateTime targetTime = DateTime.Parse(_data.LastTimer).AddMinutes(_relicData.Timer).AddSeconds(60);
        DateTime currTime = DateTime.Now;
        TimeSpan subTime = targetTime.Subtract(currTime);        

        if (subTime.Hours <= 0 && subTime.Minutes <= 0)
        {
            _txtCountdownCollect.gameObject.SetActive(false);
            _txtCoinCollect.gameObject.SetActive(true);
            _BtnCollect.interactable = true;

            _timer = null;
            OnRelicCollection?.Invoke(_relicData.Coin);
        }
        else
        {
            _txtCountdownCollect.gameObject.SetActive(true);
            _txtCoinCollect.gameObject.SetActive(false);
            _BtnCollect.interactable = false;

            _timer = new TimerUtils.CountdownTimer(subTime.Hours * 60 + subTime.Minutes);
            _timer.OnTimerStop += OnCountdownCompleted;
            _timer.Start();
        }
    }

    private void UpdateTxtName()
    {
        _txtName.text = _relicData.Name;
    }

    private void UpdateArtRelic()
    {
        _ImgRelic.sprite = ResourceManager.Instance.GetRelicSpriteByID(_relicData.ID);
    }

    private void Start()
    {
        _BtnSelecter.onClick.AddListener(OnClickSelecter);
        _BtnCollect.onClick.AddListener(OnClickCollection);

    }

    private void OnDestroy()
    {
        _BtnSelecter.onClick.RemoveListener(OnClickSelecter);
        _BtnCollect.onClick.RemoveListener(OnClickCollection);
    }

    private void Update()
    {
        if(_timer != null)
        {
            UpdateTimer(_timer.GetTime());
            _timer.Tick(Time.deltaTime);
        }
    }

    private void UpdateTimer(float minute)
    {
        TimeSpan t = TimeSpan.FromMinutes(minute);
        _txtCountdownCollect.text = $"{t.Hours}:{t.Minutes}";
    }

    private void OnClickSelecter()
    {
        GUIManager.Instance.ShowPopup<PopupRelicSelecter>(_idGallery, _idRelics, _idx, this);
    }

    private void OnClickCollection()
    {
        _data.LastTimer = DateTime.Now.ToString();
        UpdateTxtCountdownCollect();

        //TODO: Add coin for player        
    }

    private void OnCountdownCompleted()
    {
        _txtCountdownCollect.gameObject.SetActive(false);
        _txtCoinCollect.gameObject.SetActive(true);
        _timer = null;

        _BtnCollect.interactable = true;
        OnRelicCollection?.Invoke(_relicData.Coin);
        //TODO: Add coin to collect all button
    }

    public void OnCollect()
    {
        if(_ObjCollect.activeSelf == true)
        {
            if(_timer == null)
            {
                OnClickCollection();
            }
        }
    }
}
