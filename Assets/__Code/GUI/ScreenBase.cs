using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScreenBase : MonoBehaviour
{
    public bool canReused = false;

    private RectTransform m_RectTransform;
    //private float m_OffsetY = 0f;

    protected void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

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
        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            //m_OffsetY = 100f;
        }
    }

    public void OnInit(object[] paras)
    {
        //throw new NotImplementedException();
    }
}
