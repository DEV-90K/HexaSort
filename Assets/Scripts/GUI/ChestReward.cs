using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestReward : MonoBehaviour
{
    [SerializeField]
    private GameObject _Reward;
    [SerializeField]
    private GameObject _Chest;
    [SerializeField]
    private Animator _Animator;
    [SerializeField]
    private Transform _HidePoint;
    [SerializeField]
    private Button _BtnReward;
    [SerializeField]
    private Vector3 _originPos;
    [SerializeField]
    private Image _Icon;
    [SerializeField]
    private TMP_Text _Amount;

    private ChestRewardData _rewardData;
    private PopupChestReward _popup;

    public void OnInit(PopupChestReward popup, ChestRewardData data)
    {
        _popup = popup;
        _rewardData = data;

        _Icon.sprite = ResourceManager.Instance.GetRewardSpriteByType(data.Type);
        _Amount.text = "+ " + _rewardData.Amount.ToString() + " " + EnumUtils.ParseString(data.Type);

        transform.localPosition = _originPos;

        _Animator.enabled = true;
        _Animator.SetBool("IsOpen", false);
        _Animator.Play("Chest_Show");
    }

    private void Start()
    {
        _BtnReward.onClick.AddListener(OnClickReward);
    }

    private void OnDestroy()
    {
        _BtnReward.onClick.RemoveListener(OnClickReward);
    }

    private void OnClickReward()
    {
        StartCoroutine(IE_Reward());
    }

    private IEnumerator IE_Reward()
    {
        _popup.HideChests();
        LeanTween.cancel(gameObject); //make not effect to hide

        yield return new WaitForSeconds(0.2f);
        TweenMoveShow();
        yield return new WaitForSeconds(0.2f);
        yield return _Animator.IE_WaitAnimation();
        _popup.ShowButtonClaim();
    }

    public void TweenMoveHide()
    {
        _Animator.enabled = false;
        LeanTween.move(gameObject, _HidePoint.position, 0.2f);
    }

    private void TweenMoveShow()
    {
        _Animator.enabled = true;
        LeanTween.move(gameObject, transform.parent.position, 0.2f)
                .setOnComplete(() =>
                {
                    _Animator.SetBool("IsOpen", true);
                });
    }
}
