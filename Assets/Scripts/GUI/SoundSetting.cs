using UnityEngine;
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

    private void Start()
    {
        _BtnAdd.onClick.AddListener(OnClickAdd);
        _BtnSub.onClick.AddListener(OnClickSub);
        _Toggle.onValueChanged.AddListener(OnToggle);

        _volume = AudioManager.Instance.GetSoundVolume();
        UpdateSlider();
        _Toggle.isOn = AudioManager.Instance.CanSound();
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
        AudioManager.Instance.PlaySoundSetting(_volume);
    }

    private void OnClickSub() {
        _volume -= 0.1f;
        UpdateSlider();
        AudioManager.Instance.PlaySoundSetting(_volume);
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
    }

    private void OnToggle(bool toggle)
    {
        Debug.Log("Ontoggle: " + toggle);
        _Toggle.isOn = toggle;
        AudioManager.Instance.UpdateSoundState(toggle);
    }
}
