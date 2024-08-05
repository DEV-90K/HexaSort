using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GalleryRelic : MonoBehaviour
{
    public static Action<int> OnRelicClaim;
    public static Action<int> OnCountdownCompleted;

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
    private TMP_Text _txtName;

    [SerializeField]
    private Button _BtnInfo;
    [SerializeField]
    private GameObject _ObjProgress;
    [SerializeField]
    private Slider _SliderProgress;
    [SerializeField]
    private TMP_Text _txtCoutdown;

    [SerializeField]
    private GameObject _ObjCoinClaim;
    [SerializeField]
    private TMP_Text _txtCoinClaim;

    private GalleryRelicData _data;
    private RelicData _relicData;

    private TimerUtils.CountdownTimer _timer = null;
    public void OnInit(GalleryRelicData galleryRelicData, int idGallery, int[] idRelics)
    {
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

        ActiveUnLockState();
        SetUpCountdownTimer();
    }

    private void ActiveUnLockState()
    {        
        _BtnInfo.interactable = true;
    }

    private void SetUpCountdownTimer()
    {
        _ObjProgress.gameObject.SetActive(false);
        _ObjCoinClaim.gameObject.SetActive(false);

        TimeSpan subTime = GetTimeFromLastClick();
        if (subTime.Hours <= 0 && subTime.Minutes <= 0)
        {
            CountdownCompleted();
        }
        else
        {
            CountdownProgressing();
        }
    }

    private void CountdownCompleted()
    {
        _timer = null;
        _ObjCoinClaim.gameObject.SetActive(true);
        OnCountdownCompleted?.Invoke(_relicData.Coin);
    }

    private void CountdownProgressing()
    {
        _ObjProgress.gameObject.SetActive(true);

        DateTime lastTime = DateTime.Parse(_data.LastTimer);
        DateTime targetTime = lastTime.AddMinutes(_relicData.Timer).AddSeconds(60);
        DateTime currTime = DateTime.Now;
        TimeSpan time = targetTime.Subtract(lastTime);
        TimeSpan timePassed = currTime.Subtract(lastTime);
        TimeSpan timeRemaining = targetTime.Subtract(currTime);

        _SliderProgress.maxValue = (float)time.TotalMinutes;
        _SliderProgress.minValue = 0;
        _SliderProgress.value = (float)timeRemaining.TotalMinutes;
        UpdateSliderProgress(_SliderProgress.value);

        _timer = new TimerUtils.CountdownTimer((float)timeRemaining.TotalMinutes);
        _timer.OnTimerStop += OnCountdownStopped;
        _timer.Start();
        UpdateTimer();
    }

    private void UpdateSliderProgress(float value)
    {
        _SliderProgress.value = value;
    }

    private void UpdateTxtCoinClaim()
    {
        _txtCoinClaim.text = "+" + _relicData.Coin + " Coin";
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
        _BtnInfo.onClick.AddListener(OnClickInfo);
    }

    private void OnDestroy()
    {
        _BtnSelecter.onClick.RemoveListener(OnClickSelecter);
        _BtnInfo.onClick.RemoveListener(OnClickInfo);
    }

    private void Update()
    {
        if(_timer != null)
        {
            _timer.Tick(Time.deltaTime);
            UpdateSliderProgress(_SliderProgress.value - Time.deltaTime);
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        float minute = _SliderProgress.value;
        TimeSpan t = TimeSpan.FromMinutes(minute);
        _txtCoutdown.text = $"{t.Hours}h:{t.Minutes}m";
    }

    private void OnClickSelecter()
    {
        GUIManager.Instance.ShowPopup<PopupGalleryRelicSelecter>(_data);
    }

    private void OnClickClaim()
    {
        _ObjCoinClaim.gameObject.SetActive(false);
        _ObjProgress.gameObject.SetActive(true);

        DateTime lastTime = DateTime.Parse(_data.LastTimer);
        DateTime targetTime = lastTime.AddMinutes(_relicData.Timer).AddSeconds(60);
        DateTime currTime = DateTime.Now;
        TimeSpan time = targetTime.Subtract(lastTime);
        TimeSpan timePassed = currTime.Subtract(lastTime);
        TimeSpan timeRemaining = targetTime.Subtract(currTime);

        _SliderProgress.maxValue = (float)time.TotalMinutes;
        _SliderProgress.minValue = 0;
        _SliderProgress.value = (float)timeRemaining.TotalMinutes;
        UpdateSliderProgress(_SliderProgress.value);

        _timer = new TimerUtils.CountdownTimer((float)timeRemaining.TotalMinutes);
        _timer.OnTimerStop += OnCountdownStopped;
        _timer.Start();
        UpdateTimer();

        OnRelicClaim?.Invoke(_relicData.Coin);
    }

    private void OnClickInfo()
    {
        GUIManager.Instance.ShowPopup<PopupGalleryRelic>(_data);
    }

    private void OnCountdownStopped()
    {
        _timer.OnTimerStop -= OnCountdownStopped;
        _timer = null;

        _ObjProgress.gameObject.SetActive(false);
        _ObjCoinClaim.gameObject.SetActive(true);
        
        OnCountdownCompleted?.Invoke(_relicData.Coin);
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
        DateTime targetTime = DateTime.Parse(_data.LastTimer).AddMinutes(_relicData.Timer).AddSeconds(60);
        DateTime currTime = DateTime.Now;
        TimeSpan subTime = targetTime.Subtract(currTime);

        return subTime;
    }
}
