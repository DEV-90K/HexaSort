using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenChallenge : ScreenBase
{
    public static ScreenChallenge Instance;
    [SerializeField]
    private Button _btnLeft;
    [SerializeField]
    private Button _btnRight;
    [SerializeField]
    private TMP_Text _txtCoin;

    private ChallengeManager _challengeManager;

    public override void OnInit(params object[] paras)
    {
        Instance = this;
        base.OnInit(paras);
        _challengeManager = ChallengeManager.Instance;
        Show();
    }

    public override void Show()
    {
        base.Show();
        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
        UpdateTxtCoin();
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
        _challengeManager.ShowStackLeft();
        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
    }

    private void OnClickBtnRight()
    {
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
        _txtCoin.text = "0"; //MainPlayer.Instance.GetCoin().ToString();
    }

    public void OnBackBtnClick()
    {
        SceneManager.LoadScene("Tool");
    }
}
