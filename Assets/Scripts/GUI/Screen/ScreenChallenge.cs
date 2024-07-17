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

        if(!_challengeManager.CanShowLeft())
        {
            _btnLeft.gameObject.SetActive(false);
        }
        else
        {
            //When this event occurs, the left button is definitely displayed
            _btnRight.gameObject.SetActive(true);
        }
    }

    private void OnClickBtnRight()
    {
        _challengeManager.ShowStackRight();

        if (!_challengeManager.CanShowRight())
        {
            _btnRight.gameObject.SetActive(false);
        }
        else
        {
            //When this event occurs, the right button is definitely displayed
            _btnLeft.gameObject.SetActive(true);
        }
    }
}
