
using System;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [SerializeField]
    private GameObject _Content;

    public bool canReused = true;

    public virtual void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySoundShowPopup();
        LeanTween.scale(_Content, Vector3.one, 0.2f)
            .setFrom(Vector3.one * 0.63f)
            .setEaseOutBack();            
    }

    public virtual void HideByDelay(float delay)
    {
        //Anim in time delay
        Invoke(nameof(Hide), delay);
    }

    public virtual void Hide()
    {

        AudioManager.Instance.PlaySoundHidePopup();
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

    public virtual void OnInit(object[] paras)
    {
        //throw new NotImplementedException();
    }
}
