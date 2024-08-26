using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI_ScreenMain
{
    public class Gallery : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _Name;

        [SerializeField]
        private GameObject _Active;
        [SerializeField]
        private Button _Button;
        [SerializeField]
        private GameObject _SliderContainer;
        [SerializeField]
        private Slider _Slider;
        [SerializeField]
        private ParticleSystem _Particle;

        [SerializeField]
        private GameObject _Deactive;

        private GalleryData _gallery;
        private TimerUtils.CountdownTimer _timer = null;

        private void Start()
        {
            _Button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _Button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnInit(int idGallery)
        {
            _gallery = ResourceManager.Instance.GetGalleryDataByID(idGallery);

            InitState();            

            _Name.text = _gallery.Name;
        }

        private void InitState()
        {
            GalleryRelicData[] galleryRelics = MainPlayer.Instance.GetGalleryRelicByID(_gallery.ID);
            if (galleryRelics.Length > 0 || _gallery.ID == 1)
            {
                _Active.gameObject.SetActive(true);
                _Deactive.gameObject.SetActive(false);
            }
            else
            {
                if (CheckFullRelics(_gallery.ID - 1))
                {
                    _Active.gameObject.SetActive(true);
                    _Deactive.gameObject.SetActive(false);
                }
                else
                {
                    _Active.gameObject.SetActive(false);
                    _Deactive.gameObject.SetActive(true);
                }
            }

            SetUpSlider();
        }

        private void SetUpSlider()
        {
            if(_Deactive.gameObject.activeSelf)
            {
                SliderNotUsed();
                return;
            }

            GalleryRelicData[] galleryRelics = MainPlayer.Instance.GetGalleryRelicByID(_gallery.ID);
            if (galleryRelics == null || galleryRelics.Length == 0)
            {
                SliderNotUsed();
                return;
            }

            GalleryRelicData data = null;
            float minTime = float.PositiveInfinity;

            foreach (GalleryRelicData galleryRelicData in galleryRelics)
            {
                float time = GetTimeFromLastClick(galleryRelicData);
                if(time == 0)
                {
                    SliderCompleted();
                    return;
                }

                if(time < minTime)
                {
                    data = galleryRelicData;
                }                
            }

            SliderWaiting(data);
        }

        private void SliderNotUsed()
        {
            _SliderContainer.SetActive(false);
            _Particle.Stop();
        }

        private void SliderCompleted()
        {
            _SliderContainer.SetActive(false);
            _Particle.Play();
        }


        private void SliderWaiting(GalleryRelicData data)
        {
            _SliderContainer.SetActive(true);
            _Particle.Stop();

            RelicData relicData = ResourceManager.Instance.GetRelicDataByID(data.IDRelic);
            DateTime lastTime = DateTime.Parse(data.LastTimer);
            DateTime targetTime = lastTime.AddMinutes(relicData.Timer).AddSeconds(60);
            DateTime currTime = DateTime.Now;
            TimeSpan time = targetTime.Subtract(lastTime);
            TimeSpan timePassed = currTime.Subtract(lastTime);
            TimeSpan timeRemaining = targetTime.Subtract(currTime);

            _Slider.maxValue = (float)time.TotalMinutes;
            _Slider.minValue = 0;
            _Slider.value = (float)timeRemaining.TotalMinutes;

            _timer = new TimerUtils.CountdownTimer((float)timeRemaining.TotalMinutes);
            _timer.OnTimerStop += OnCountdownStopped;
            _timer.Start();
        }

        private void OnCountdownStopped()
        {
            _timer.OnTimerStop -= OnCountdownStopped;
            _timer = null;

            SliderCompleted();
        }

        private float GetTimeFromLastClick(GalleryRelicData galleryRelicData)
        {
            RelicData relicData = ResourceManager.Instance.GetRelicDataByID(galleryRelicData.IDRelic);
            DateTime lastTimeClick = DateTime.Parse(galleryRelicData.LastTimer);
            DateTime targetTime = lastTimeClick.AddMinutes(relicData.Timer).AddSeconds(60);
            DateTime currTime = DateTime.Now;
            TimeSpan subTime = targetTime.Subtract(currTime);

            return (float) subTime.TotalMinutes;
        }

        private bool CheckFullRelics(int idGallery)
        {
            GalleryData galleryData = ResourceManager.Instance.GetGalleryDataByID(idGallery);
            GalleryRelicData[] galleryRelicDatas = MainPlayer.Instance.GetGalleryRelicByID(idGallery);

            if(galleryData.IDRelics.Length == galleryRelicDatas.Length)
            {
                return true;
            }

            return false;
        }

        private void OnButtonClick()
        {
<<<<<<< HEAD
            Debug.Log("Show Gallery no callback");
            PopupGallery popup = PopupManager.Instance.ShowPopup<PopupGallery>();
            popup.OnInit(_gallery.ID);
=======
            GUIManager.Instance.ShowPopup<PopupGallery>(_gallery.ID);
>>>>>>> parent of 99c86cc (Update)
        }

        private void Update()
        {
            if (_timer != null)
            {
                _timer.Tick(Time.deltaTime / 60f);
                _Slider.value -= Time.deltaTime / 60f;
            }
        }
    }
}
