using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    public bool canReused = false;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideByDelay(float delay)
    {
        //Anim in time delay
        Invoke(nameof(Hide), delay);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if (!canReused)
        {
            Destroy(gameObject);
        }
    }

    public void OnSetup()
    {
        throw new NotImplementedException();
    }

    public void OnInit(object[] paras)
    {
        throw new NotImplementedException();
    }
}
