using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkip : MonoBehaviour
{
    private RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        OnSetup();
    }

    public void OnSetup()
    {
        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 1920 / 1080f)
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
}
