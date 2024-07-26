using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryRelic : MonoBehaviour
{
    public static Action<int> OnRelicClaim;
    public static Action<int> OnCoutDownCompleted;

    [Header("Model Selecter")]
    [SerializeField]
    private GameObject _ObjSelecter;
    [SerializeField]
    private Button _BtnSelecter;

    [Header("Model Collect")]
    [SerializeField]
    private GameObject _ObjCollect;
    [SerializeField]
    private Image _ImgRelic;
    [SerializeField]
    private TMP_Text _txtCoutdown;
    [SerializeField]
    private TMP_Text _txtClaimCoin;
    [SerializeField]
    private TMP_Text _txtName;
    [SerializeField]
    private Button _BtnClaim;
    [SerializeField]
    private Button _BtnCollect;

    [SerializeField]
    private Button _BtnInfo;

    private GalleryRelicData _data;
    private RelicData _relicData;

    //private int _idx;
    //public int[] _idRelics;
    //private int _idGallery;

    private TimerUtils.CountdownTimer _timer = null;
    public void OnInit(GalleryRelicData galleryRelicData, int idGallery, int[] idRelics)
    {
        //_idx = transform.GetSiblingIndex();
        //_idGallery = idGallery;
        //_idRelics = idRelics;

        _data = galleryRelicData;

        if (_data == null)
        {
            _data = new GalleryRelicData();
            _data.State = GalleryRelicState.NONE;
            _data.IDGallery = idGallery;
            _data.Position = transform.GetSiblingIndex();
            _data.IDRelic = -1; //Not Contain
            _data.LastTimer = "";

            SetUpModelSelecter();
        }
        else
        {
            _relicData = ResourceManager.Instance.GetRelicDataByID(_data.IDRelic);
            SetUpModelCollection();
        }
    }

    private void SetUpModelSelecter()
    {
        _timer = null;
        _ObjCollect.SetActive(false);
        _ObjSelecter.SetActive(true);
    }

    private void SetUpModelCollection()
    {
        _ObjSelecter.SetActive(false);
        _ObjCollect.SetActive(true);

        UpdateTxtName();
        UpdateArtRelic();
        UpdateTxtCoinClaim();

        if (_data.State == GalleryRelicState.LOCK)
        {
            _BtnClaim.gameObject.SetActive(false);
            _txtCoutdown.gameObject.SetActive(false);
            _BtnCollect.gameObject.SetActive(true);
            _BtnInfo.interactable = false;
            return;
        }
        else
        {
            _BtnCollect.gameObject.SetActive(false);
            _BtnInfo.interactable = true;
        }

        TimeSpan subTime = GetTimeFromLastClick();
        if (subTime.Hours <= 0 && subTime.Minutes <= 0)
        {
            _timer = null;
            _BtnClaim.gameObject.SetActive(true);
            _txtCoutdown.gameObject.SetActive(false);            

            OnCoutDownCompleted?.Invoke(_relicData.Coin);
        }
        else
        {
            _timer = new TimerUtils.CountdownTimer(subTime.Hours * 60 + subTime.Minutes);
            _timer.OnTimerStop += OnCountdownCompleted;
            _timer.Start();

            _BtnClaim.gameObject.SetActive(false);
            _txtCoutdown.gameObject.SetActive(true);
        }        
    }

    private void UpdateTxtCoinClaim()
    {
        _txtClaimCoin.text = "+" + _relicData.Coin;
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
        _BtnClaim.onClick.AddListener(OnClickClaim);
        _BtnInfo.onClick.AddListener(OnClickInfo);
        _BtnCollect.onClick.AddListener(OnClickCollect);

    }

    private void OnDestroy()
    {
        _BtnSelecter.onClick.RemoveListener(OnClickSelecter);
        _BtnClaim.onClick.RemoveListener(OnClickClaim);
        _BtnInfo.onClick.RemoveListener(OnClickInfo);
        _BtnCollect.onClick.RemoveListener(OnClickCollect);
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
        _txtCoutdown.text = $"{t.Hours}h:{t.Minutes}m";
    }

    private void OnClickSelecter()
    {
        GUIManager.Instance.ShowPopup<PopupGalleryRelicSelecter>(_data);
    }

    private void OnClickClaim()
    {
        //TODO: Add coin to player
        _data.LastTimer = DateTime.Now.ToString();
        TimeSpan timeSpan = GetTimeFromLastClick();
        _timer = new TimerUtils.CountdownTimer(timeSpan.Hours * 60 + timeSpan.Minutes);
        _timer.OnTimerStop += OnCountdownCompleted;
        _timer.Start();

        _BtnClaim.gameObject.SetActive(false);
        _txtCoutdown.gameObject.SetActive(true);

        OnRelicClaim?.Invoke(_relicData.Coin);
    }

    private void OnClickInfo()
    {
        GUIManager.Instance.ShowPopup<PopupGalleryRelic>(_data);
    }

    private void OnClickCollect()
    {
        ChallengeManager.Instance.OnInit(_data);
        GUIManager.Instance.HidePopup<PopupGallery>();
        GUIManager.Instance.HideScreen<ScreenMain>();
    }

    private void OnCountdownCompleted()
    {
        _timer.OnTimerStop -= OnCountdownCompleted;
        _timer = null;

        _BtnClaim.gameObject.SetActive(true);
        _txtCoutdown.gameObject.SetActive(false);

        OnCoutDownCompleted?.Invoke(_relicData.Coin);
    }

    public void OnClaim()
    {
        if(_ObjCollect.activeSelf == true)
        {
            if(_timer == null)
            {
                OnClickClaim();
            }
        }
    }

    private TimeSpan GetTimeFromLastClick()
    {
        //DateTime targetTime = _data.LastTimeSpinFreeClicked.AddMinutes(timeToFree).AddSeconds(60); //Countdown => need add 60
        DateTime targetTime = DateTime.Parse(_data.LastTimer).AddMinutes(_relicData.Timer).AddSeconds(60);
        DateTime currTime = DateTime.Now;
        TimeSpan subTime = targetTime.Subtract(currTime);

        return subTime;
    }
}
