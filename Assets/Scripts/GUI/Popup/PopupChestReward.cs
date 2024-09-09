using GUIChestReward;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupChestReward : PopupBase
{
    [SerializeField]
    private ChestController _chestControl;
    [SerializeField]
    private TMP_Text _txtTutorial;
    [SerializeField]
    private ChestReward[] _chestRewards;
    [SerializeField]
    private Button _BtnContinue;
    [SerializeField]
    private TMP_Text _TxtContinue;
    [SerializeField]
    private GUIChestReward.Reward _Reward;

    [SerializeField]
    private Transform _rewardStart;
    [SerializeField]
    private Transform _rewardEnd;

    private ChestRewardData _rewardData;

    private void OnClickContinue()
    {
        DisableEventClickContinue();
        int random = Random.Range(0, _chestRewards.Length);
        _chestRewards[random].OnClickReward();
    }

    private void DisableEventClickContinue()
    {
        _BtnContinue.onClick.RemoveListener(OnClickContinue);
        _TxtContinue.gameObject.SetActive(false);
    }

    private void EnableEventClickContinue()
    {
        _BtnContinue.onClick.AddListener(OnClickContinue);
        _TxtContinue.gameObject.SetActive(true);
    }

    public void OnInitReward()
    {
        //GUIChestReward.Reward reward = Instantiate(_reward, transform);
        _Reward.gameObject.SetActive(true);
        _Reward.OnInit(_rewardData, _rewardStart.position, _rewardEnd.position);
        Claim();

        this.Invoke(() =>
        {
            _Reward.gameObject.SetActive(false);
            Hide();
        }, 2.5f);
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

    public void HideChests()
    {
        DisableEventClickContinue();

        foreach (ChestReward chest in _chestRewards)
        {
            chest.TweenMoveHide();
        }
    }

    public void HideTutorial()
    {
        _txtTutorial.gameObject.SetActive(false);        
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

        EnableEventClickContinue();
        _txtTutorial.gameObject.SetActive(true);
        _Reward.gameObject.SetActive(false);
    }

    public override void Hide()
    {
        base.Hide();
        DisableEventClickContinue();
    }
}
