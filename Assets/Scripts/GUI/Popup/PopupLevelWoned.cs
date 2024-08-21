using Audio_System;
using CollectionSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevelWoned : PopupBase
{
    [SerializeField]
    private CoinManager _CoinManager;
    [SerializeField]
    private MaterialManager _MaterialManager;
    [SerializeField]
    private Button _BtnReward;
    [SerializeField]
    private TMP_Text _TextCoin;
    [SerializeField]
    private TMP_Text _TextMaterial;
        
    private LevelPresenterData _presenterData;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);        
        _presenterData = (LevelPresenterData)paras[0];        
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtCoin(_presenterData.Coin);
        UpdateTxtMaterial(_presenterData.Material);
    }

    private void Start()
    {
        _BtnReward.onClick.AddListener(OnClickBtnReward);
    }

    private void OnDestroy()
    {
        _BtnReward.onClick.RemoveListener(OnClickBtnReward);
    }

    private void OnClickBtnReward()
    {
        SFX_ClickReward();
        StartCoroutine(IE_VFX_Claim());        
    }

    private void SFX_ClickReward()
    {
        SoundData soundData = SoundResource.Instance.ButtonCollectCoin;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }

    private IEnumerator IE_VFX_Claim()
    {
        _CoinManager.VFX_ShowCoin(_presenterData.Coin);
        _MaterialManager.VFX_ShowMaterial(_presenterData.Material);
        float timeCoin = _CoinManager.GetTime();
        float timeMaterial = _MaterialManager.GetTime();

        _BtnReward.interactable = false;
        yield return new WaitForSeconds(timeCoin);
        _BtnReward.interactable = true;

        int[] relicNotOwner = ResourceManager.Instance.GetRelicDatasPlayerNotOwner(1);
        if (relicNotOwner.Length > 0)
        {
            for (int i = 0; i < relicNotOwner.Length; i++)
            {
                RelicData data = ResourceManager.Instance.GetRelicDataByID(relicNotOwner[i]);
                if (data.Material <= MainPlayer.Instance.GetMaterial())
                {
                    DialogueData dialogueData = ResourceManager.Instance.GetDialogueDataByType(DialogueType.RELIC_COLLECT);
                    System.Action callback = () =>
                    {
                        LevelManager.Instance.OnInitLevelByID(_presenterData.Level + 1);
                        Hide();
                    };

                    DialogueManager.Instance.ShowDialougeBox(dialogueData, callback);
                    yield break;
                }
            }
        }

        LevelManager.Instance.OnInitCurrentLevel();
        Hide();
    }      

    private void UpdateTxtMaterial(int mal)
    {
        if(mal <= 0)
        {
            _TextMaterial.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            _TextMaterial.transform.parent.gameObject.SetActive(true);
        }

        _TextMaterial.text = "+" + mal;
    }
    private void UpdateTxtCoin(int coin)
    {
        if (coin <= 0)
        {
            _TextCoin.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            _TextCoin.transform.parent.gameObject.SetActive(true);
        }

        _TextCoin.text = "+" + coin;
    }
}
