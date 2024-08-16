using Audio_System;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField]
    private Button _BtnAdd;
    [SerializeField]
    private Button _BtnSub;
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
        _BtnAdd.onClick.AddListener(OnClickAdd);
        _BtnSub.onClick.AddListener(OnClickSub);
        _Toggle.onValueChanged.AddListener(OnToggle);
    }

    private void OnDestroy()
    {
        _BtnAdd.onClick.RemoveListener(OnClickAdd);
        _BtnSub.onClick.RemoveListener(OnClickSub);
        _Toggle.onValueChanged.RemoveListener(OnToggle);
    }

    private void OnClickAdd()
    {
        _volume += 0.1f;
        UpdateSlider();
    }

    private void OnClickSub() {
        _volume -= 0.1f;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if(_volume >= 1f)
        {
            _BtnAdd.interactable = false;
        }
        else if(_volume <= 0f)
        {
            _BtnSub.interactable = false;
        }
        else
        {
            _BtnAdd.interactable = true;
            _BtnSub.interactable = true;            
        }

        _Slider.value = _volume;
        SoundManager.Instance.Volumne(_Slider.value);
    }

    private void OnToggle(bool toggle)
    {
        _Toggle.isOn = toggle;
        SoundManager.Instance.CanSound = toggle;
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
