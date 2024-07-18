using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChallenge : ScreenBase
{
    [SerializeField]
    private Button _btnLeft;
    [SerializeField]
    private Button _btnRight;

    private ChallengeManager _challengeManager;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        _challengeManager = ChallengeManager.Instance;

        UpdateStateOfButtonRight();
        UpdateStateOfButtonLeft();
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
}
