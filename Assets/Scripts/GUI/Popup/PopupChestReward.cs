using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupChestReward : PopupBase
{
    [SerializeField]
    private ChestController _chestControl;
    [SerializeField]
    private ChestReward[] _chestRewards;

    private ChestRewardData _rewardData;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);        
        _rewardData = _chestControl.GetChestRewardData();

        foreach (ChestReward chestReward in _chestRewards)
        {
            chestReward.OnInit(_rewardData);
        }
    }

    public override void Show()
    {
        base.Show();
        _rewardData.DebugLogObject();
    }
}
