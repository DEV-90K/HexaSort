using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChallenge : ScreenBase
{
    [SerializeField]
    private Button _btnLeft;
    [SerializeField]
    private Button _btnRight;
    [SerializeField]
    private TMP_Text _txtCoin;
    [SerializeField]
    private TMP_Text _txtMaterial;

    private ChallengeManager _challengeManager;
    private int _coin;
    private int _material;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        _challengeManager = ChallengeManager.Instance;
        _coin = MainPlayer.Instance.GetCoin();
        _material = MainPlayer.Instance.GetMaterial();
        Show();
    }

    public override void Show()
    {
        base.Show();
        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
        UpdateTxtCoin();
        UpdateTxtMaterial();
        GameManager.Instance.ChangeState(GameState.CHALLENGE_PLAYING);
    }

    public override void Hide()
    {
        base.Hide();        
        GameManager.Instance.ChangeState(GameState.FINISH);
    }

    private void OnEnable()
    {
        StackChallengeManager.OnAllStackPlaced += SCM_OnAllStackPlaced;
    }

    private void OnDisable()
    {
        StackChallengeManager.OnAllStackPlaced -= SCM_OnAllStackPlaced;
    }

    private void SCM_OnAllStackPlaced()
    {
        if(_btnRight.gameObject.activeSelf)
        {
            OnClickBtnRight();
        }
        else
        {
            OnClickBtnLeft();
        }
    }

    private void Start()
    {
        _btnLeft.onClick.AddListener(OnClickBtnLeft);
        _btnRight.onClick.AddListener(OnClickBtnRight);
    }

    private void OnDestroy()
    {
        _btnLeft.onClick.RemoveListener(OnClickBtnLeft);
        _btnRight.onClick.RemoveListener(OnClickBtnRight);
    }

    private void OnClickBtnLeft()
    {
        if (GameManager.Instance.IsState(GameState.PAUSE))
        {
            return;
        }

        _challengeManager.ShowStackLeft();
        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
    }

    private void OnClickBtnRight()
    {
        if (GameManager.Instance.IsState(GameState.PAUSE))
        {
            return;
        }

        _challengeManager.ShowStackRight();
        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
    }

    private void UpdateStateOfButtonRight()
    {
        _btnRight.gameObject.SetActive(_challengeManager.CanShowRight());
    }

    private void UpdateStateOfButtonLeft()
    {
        _btnLeft.gameObject.SetActive(_challengeManager.CanShowLeft());
    }

    private void UpdateTxtCoin()
    {
        _txtCoin.text = _coin.ToString();
    }

    private void UpdateTxtMaterial()
    {
        _txtMaterial.text = _material.ToString();
    }
}
