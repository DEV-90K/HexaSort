using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoostRefresh : MonoBehaviour, IBoostTutorial
{
    [SerializeField]
    private GameObject _ObjNoti;
    [SerializeField]
    private TMP_Text _txtAmount;
    [SerializeField]
    private CanvasGroup _group;

    private Button mButton;

    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        MainPlayer.OnChangeRefresh += MainPlayer_OnChangeRefresh;
    }

    private void OnDestroy()
    {
        MainPlayer.OnChangeRefresh -= MainPlayer_OnChangeRefresh;
    }

    private void MainPlayer_OnChangeRefresh(int amount)
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

    public void OnInit()
    {
        Debug.Log("On Init Boost Refresh");
        int amount = MainPlayer.Instance.GetRefresh();
        MainPlayer_OnChangeRefresh(amount);
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

        LevelManager.Instance.OnBoostRefresh();
    }

    public void ShowBoostTutorial()
    {
        LeanTween.alphaCanvas(_group, 0.6f, 1f)
            .setFrom(1f)
            .setLoopPingPong();
    }

    public void HideBoostTutorial()
    {
        LeanTween.cancel(gameObject);
        _group.alpha = 1f;
    }
}
