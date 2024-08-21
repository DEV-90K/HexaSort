using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(Button))]
public class ButtonBase : MonoBehaviour
{
    private Button _Button;

    private void Awake()
    {
        _Button = GetComponent<Button>();
    }

    private void Start()
    {
        _Button.onClick.AddListener(SFX_ButtonClick);
        _Button.onClick.AddListener(VFX_ButtonClick);
        _Button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _Button.onClick.RemoveListener(SFX_ButtonClick);
        _Button.onClick.RemoveListener(VFX_ButtonClick);
        _Button.onClick.RemoveListener(OnButtonClick);
    }

    public virtual void SFX_ButtonClick()
    {

    }

    public virtual void VFX_ButtonClick()
    {
        //Noob
    }

    public virtual void OnButtonClick()
    {
        //Noob
    }
}
