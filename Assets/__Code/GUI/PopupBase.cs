
using System;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public bool canReused = true;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideByDelay(float delay)
    {
        //Anim in time delay
        Invoke(nameof(Hide), delay);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);

        if(!canReused)
        {
            Destroy(gameObject);
        }
    }

    public void OnSetup()
    {
        //throw new NotImplementedException();
    }

    public virtual void OnInit(object[] paras)
    {
        //throw new NotImplementedException();
    }
}
