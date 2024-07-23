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
