
using Audio_System;
using Unity.VisualScripting;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [SerializeField]
    private GameObject _Content;

    public bool canReused = true;

    public virtual void Show()
    {
        gameObject.SetActive(true);

        SFX_ShowPopup();
        VFX_ShowPopup();            
    }

    public virtual void VFX_ShowPopup()
    {
        LeanTween.scale(_Content, Vector3.one, 0.2f)
            .setFrom(Vector3.one * 0.63f)
            .setEaseOutBack();
    }

    public virtual void SFX_ShowPopup()
    {
        SoundData soundData = SoundResource.Instance.ShowPopup;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }

    public virtual void HideByDelay(float delay) 
    {
        //Anim in time delay
        Invoke(nameof(Hide), delay);
    }

    private void Hide()
    {
        SFX_HidePopup();
        VFX_HidePopup();
    }

    public virtual void SFX_HidePopup()
    {
        SoundData soundData = SoundResource.Instance.HidePopup;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }

    public virtual void VFX_HidePopup()
    {
        LeanTween.scale(_Content, Vector3.one * 0.63f, 0.2f)
            .setFrom(Vector3.one)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
                if (!canReused)
                {                    
                    Destroy(gameObject);
                }
            });
    }

    public virtual void OnSetup()
    {
        //throw new NotImplementedException();
    }

    //public virtual void OnInit(object[] paras)
    //{
    //    //throw new NotImplementedException();
    //}
}
