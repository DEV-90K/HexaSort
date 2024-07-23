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

    private ChestRewardData _data;
    public void OnInit(ChestRewardData rewardData)
    {
        _data = rewardData;

        UpdateImageItem();
        UpdateTextAmount();
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
    }
}
