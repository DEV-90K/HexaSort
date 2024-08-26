using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupChestReward : PopupBase
{
    [SerializeField]
    private Button _btnClaim;
    [SerializeField]
    private Button _btnClose;
    [SerializeField]
    private ChestController _chestControl;
    [SerializeField]
    private TMP_Text _txtTutorial;
    [SerializeField]
    private ChestReward[] _chestRewards;

    private ChestRewardData _rewardData;

    private void Start()
    {
        _btnClaim.onClick.AddListener(OnClickClaim);
        _btnClose.onClick.AddListener(OnClickClose);
    }

    private void OnDestroy()
    {
        _btnClaim.onClick.RemoveListener(OnClickClaim);
        _btnClose.onClick.RemoveListener(OnClickClose);
    }

    public void OnClickClaim()
    {
        Claim();
        PopupManager.Instance.HidePopup<PopupChestReward>();
    }

    private void Claim()
    {
        MainPlayer.Instance.UpdateChestLastTime();

        if (_rewardData.Type == RewardType.COIN)
        {
            MainPlayer.Instance.AddCoin(_rewardData.Amount);
        }
        else if (_rewardData.Type == RewardType.MATERIAL)
        {
            MainPlayer.Instance.AddMaterial(_rewardData.Amount);
        }
        else if (_rewardData.Type == RewardType.SWAP)
        {
            MainPlayer.Instance.AddSwap(_rewardData.Amount);
        }
        else if (_rewardData.Type == RewardType.REFRESH)
        {
            MainPlayer.Instance.AddRefresh(_rewardData.Amount);
        }
        else if (_rewardData.Type == RewardType.HAMMER)
        {
            MainPlayer.Instance.AddHammer(_rewardData.Amount);
        }

        GUIManager.Instance.GetScreen<ScreenMain>().Show();
    }

    public void OnClickClose()
    {
        if(_btnClaim.gameObject.activeSelf)
        {
            Claim();
        }

        PopupManager.Instance.HidePopup<PopupChestReward>();
    }
    public void HideChests()
    {
        foreach (ChestReward chest in _chestRewards)
        {
            chest.TweenMoveHide();
        }
    }

    public void ShowButtonClaim()
    {
        _txtTutorial.gameObject.SetActive(false);
        _btnClaim.gameObject.SetActive(true);
    }

    public void OnInit()
    {
        _rewardData = _chestControl.GetChestRewardData();

        foreach (ChestReward chestReward in _chestRewards)
        {
            chestReward.OnInit(this, _rewardData);
        }
    }

    public override void Show()
    {
        base.Show();
        _txtTutorial.gameObject.SetActive(true);
        _btnClaim.gameObject.SetActive(false);
    }
}
