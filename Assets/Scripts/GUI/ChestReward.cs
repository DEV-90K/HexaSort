using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestReward : MonoBehaviour
{
    [SerializeField]
    private Image _item;
    [SerializeField]
    private TMP_Text _txtAmount;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider _collider;

    private ChestRewardData _data;
    private PopupChestReward _owner;
    public void OnInit(PopupChestReward popupChestReward, ChestRewardData rewardData)
    {
        _owner = popupChestReward;  
        _data = rewardData;
        _collider.enabled = true;
        _animator.SetBool("IsOpening", false);
        UpdateImageItem();
        UpdateTextAmount();
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void UpdateImageItem()
    {
        _item.sprite = ResourceManager.Instance.GetRewardSpriteByType(_data.Type);
    }

    private void UpdateTextAmount()
    {
        _txtAmount.text = _data.Amount.ToString();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("On Mouse Up As Button");
        _animator.SetBool("IsOpening", true);
        _owner.PreventInteraction();
    }
}
