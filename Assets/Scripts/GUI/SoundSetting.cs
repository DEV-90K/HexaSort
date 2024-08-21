using Audio_System;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField]
    private Slider _Slider;
    [SerializeField]
    private Toggle _Toggle;

    private float _volume;    

    public void OnInit(float vol, bool canSound)
    {
        OnToggle(canSound);
        _volume = vol;
        UpdateSlider();
    }

    private void Start()
    {
        _Toggle.onValueChanged.AddListener(OnToggle);
        _Slider.onValueChanged.AddListener(OnSlider);
    }

    private void OnDestroy()
    {
        _Toggle.onValueChanged.RemoveListener(OnToggle);
        _Slider.onValueChanged.RemoveListener(OnSlider);
    }

    private void UpdateSlider()
    {
        _Slider.value = _volume;
        SoundManager.Instance.Volumne(_Slider.value);
    }

    private void OnToggle(bool toggle)
    {
        _Toggle.isOn = toggle;
        _Slider.interactable = toggle;
        SoundManager.Instance.CanSound = toggle;
    }

    private void OnSlider(float val)
    {
        _volume = val;
        UpdateSlider();
    }

    public float GetVol()
    {
        return _volume;
    }

    public bool CheckCanSound()
    {
        return _Toggle.isOn;
    }
}
