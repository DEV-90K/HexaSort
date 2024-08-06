using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GadgetHeader : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _AmountCoin;
    [SerializeField]
    private TMP_Text _AmountMaterial;
    
    private RectTransform m_RectTransform;

    private void OnEnable()
    {
        MainPlayer.OnChangeCoin += MainPlayer_OnChangeCoin;
        MainPlayer.OnChangeMaterial += MainPlayer_OnChangeMaterial;
    }

    private void OnDisable()
    {
        MainPlayer.OnChangeCoin -= MainPlayer_OnChangeCoin;
        MainPlayer.OnChangeMaterial -= MainPlayer_OnChangeMaterial;
    }

    private void MainPlayer_OnChangeCoin(int amount)
    {
        UpdateAmountCoin(amount);
    }

    private void MainPlayer_OnChangeMaterial(int amount)
    {
        UpdateAmountMaterial(amount);
    }

    protected void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        OnSetup();
    }

    private void Start()
    {
        UpdateAmountCoin(MainPlayer.Instance.GetCoin());
        UpdateAmountMaterial(MainPlayer.Instance.GetMaterial());
    }

    private void UpdateAmountCoin(int amount)
    {
        _AmountCoin.text = amount.ToString();
    }

    private void UpdateAmountMaterial(int amount)
    {
        _AmountMaterial.text = amount.ToString();
    }

    private void OnSetup()
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
