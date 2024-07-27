using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoostSwap : MonoBehaviour, IBoostTrick
{
    [SerializeField]
    private GameObject _ObjNoti;
    [SerializeField]
    private TMP_Text _txtAmount;
    [SerializeField]
    private CanvasGroup _group;

    private Button mButton;

    private int _amount;
    private IBoostSwap _able;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        MainPlayer.OnChangeSwap += MainPlayer_OnChangeSwap;
    }

    private void OnDestroy()
    {
        MainPlayer.OnChangeSwap -= MainPlayer_OnChangeSwap;
    }

    private void MainPlayer_OnChangeSwap(int amount)
    {
        if (amount > 0)
        {
            _ObjNoti.SetActive(true);
            mButton.interactable = true;
            UpdateTxtAmount(amount);
        }
        else
        {
            mButton.interactable = false;
            _ObjNoti.SetActive(false);
        }
    }

    public void OnInit(IBoostSwap able)
    {
        _able = able;
        int amount = MainPlayer.Instance.GetSwap();
        MainPlayer_OnChangeSwap(amount);
    }

    private void UpdateTxtAmount(int amount)
    {
        _txtAmount.text = amount.ToString();
    }

    private void OnClickButton()
    {
        if (GameManager.Instance.IsState(GameState.PAUSE))
        {
            return;
        }

        GUIManager.Instance.ShowPopup<PopupBoostSwap>(_able);
        GUIManager.Instance.HideScreen<ScreenLevel>();
    }

    public void ShowBoostTrick()
    {
        LeanTween.alphaCanvas(_group, 0.6f, 1f)
            .setFrom(1f)
            .setLoopPingPong();
    }

    public void HideBoostTrick()
    {
        LeanTween.cancel(gameObject);
        _group.alpha = 1f;
    }
}
