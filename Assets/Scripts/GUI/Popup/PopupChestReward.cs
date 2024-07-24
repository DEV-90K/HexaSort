using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupChestReward : PopupBase
{
    [SerializeField]
    private Button _btnClaim;
    [SerializeField]
    private ChestController _chestControl;
    [SerializeField]
    private ChestReward[] _chestRewards;

    private ChestRewardData _rewardData;

    private void Start()
    {
        _btnClaim.onClick.AddListener(OnClickClaim);
    }

    private void OnDestroy()
    {
        _btnClaim.onClick.RemoveListener(OnClickClaim);
    }

    public void OnClickClaim()
    {
        MainPlayer.Instance.UpdateChestLastTime();

        if(_rewardData.Type == RewardType.COIN)
        {
            MainPlayer.Instance.AddCoin(_rewardData.Amount);
        }
        else if(_rewardData.Type == RewardType.MATERIAL)
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

        Hide();
    }

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);        
        _rewardData = _chestControl.GetChestRewardData();

        foreach (ChestReward chestReward in _chestRewards)
        {
            chestReward.OnInit(this, _rewardData);
        }
    }

    public override void Show()
    {
        base.Show();
        _rewardData.DebugLogObject();
    }

    public void PreventInteraction()
    {
        foreach (ChestReward chest in _chestRewards)
        {
            chest.DisableCollider();
        }
    }
}
