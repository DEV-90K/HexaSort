using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private void Awake()
    {
        OnInit();
    }
    private void OnInit()
    {
        ChestRewardConfigs = ResourceManager.Instance.GetChestRewardConfig();
        ChestRewardConfigs = SortConfigsByProbability();
    }

    public ChestRewardData GetChestRewardData()
    {
        if(CheckAnyCondition())
        {
            return GetChestRewardDataByCondition();
        }

        return GetChestRewardDataByProbability();
    }

    #region Chest Probability Strategy
    private ChestRewardConfig[] ChestRewardConfigs;

    private ChestRewardConfig[] SortConfigsByProbability()
    {
        IEnumerable<ChestRewardConfig> query = ChestRewardConfigs.OrderBy(config => config.Probability);
        return query.ToArray();
    }

    private ChestRewardData GetChestRewardDataByProbability()
    {
        ChestRewardConfig config = null;
        int probability = UnityEngine.Random.Range(0, 100);
        int total = 0;
        for(int i = 0; i < ChestRewardConfigs.Length; i++)
        {
            total += ChestRewardConfigs[i].Probability;

            if(total > probability)
            {
                config = ChestRewardConfigs[i];
                break;
            }
        }

        if(config == null)
        {
            return null;
        }

        ChestRewardData data = new ChestRewardData();
        data.Type = config.Type;
        data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);

        return data;
    }

    #endregion Chest Probability Strategy

    #region Chest Condition Strategy
    private ChestRewardData GetChestRewardDataByCondition()
    {
        int coin = MainPlayer.Instance.GetCoin();
        if (coin <= 0)
        {
            ChestRewardConfig config = GetConfigByType(RewardType.COIN);
            ChestRewardData data = new ChestRewardData();
            data.Type = config.Type;
            data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);
            return data;
        }
        int material = MainPlayer.Instance.GetMaterial();
        if (material <= 0)
        {
            ChestRewardConfig config = GetConfigByType(RewardType.MATERIAL);
            ChestRewardData data = new ChestRewardData();
            data.Type = config.Type;
            data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);
            return data;
        }
        int hammer = MainPlayer.Instance.GetHammer();
        if (hammer <= 0)
        {
            ChestRewardConfig config = GetConfigByType(RewardType.HAMMER);
            ChestRewardData data = new ChestRewardData();
            data.Type = config.Type;
            data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);
            return data;
        }
        int refresh = MainPlayer.Instance.GetRefresh();
        if (refresh <= 0)
        {
            ChestRewardConfig config = GetConfigByType(RewardType.REFRESH);
            ChestRewardData data = new ChestRewardData();
            data.Type = config.Type;
            data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);
            return data;
        }
        int swap = MainPlayer.Instance.GetSwap();
        if (swap <= 0)
        {
            ChestRewardConfig config = GetConfigByType(RewardType.SWAP);
            ChestRewardData data = new ChestRewardData();
            data.Type = config.Type;
            data.Amount = UnityEngine.Random.Range(config.AmountClampf[0], config.AmountClampf[1]);
            return data;
        }

        return null;
    }

    private bool CheckAnyCondition()
    {
        int coin = MainPlayer.Instance.GetCoin();
        if (coin <= 0)
        {
            return true;
        }
        int material = MainPlayer.Instance.GetMaterial();
        if (material <= 0)
        {
            return true;
        }
        int hammer = MainPlayer.Instance.GetHammer();
        if (hammer <= 0)
        {
            return true;
        }
        int refresh = MainPlayer.Instance.GetRefresh();
        if (refresh <= 0)
        {
            return true;
        }
        int swap = MainPlayer.Instance.GetSwap();
        if (swap <= 0)
        {
            return true;
        }
        return false;
    }

    private ChestRewardConfig GetConfigByType(RewardType type)
    {
        foreach (ChestRewardConfig config in ChestRewardConfigs)
        {
            if(config.Type == type)
            {
                return config;
            }
        }

        return null;
    }

    #endregion Chest Condition Strategy
}
