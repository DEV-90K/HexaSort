using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevelWoned : PopupBase
{
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
        GameManager.Instance.ChangeState(GameState.PAUSE);
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
        GameManager.Instance.ChangeState(GameState.FINISH);

        MainPlayer.Instance.AddCoin(_presenterData.Coin);
        MainPlayer.Instance.AddMaterial(_presenterData.Material);
        
        int[] relicNotOwner = ResourceManager.Instance.GetRelicDatasPlayerNotOwner(1);

        if (relicNotOwner.Length > 0)
        {
            for (int i = 0; i < relicNotOwner.Length; i++)
            {
                RelicData data = ResourceManager.Instance.GetRelicDataByID(relicNotOwner[i]);
                if(data.Material <= MainPlayer.Instance.GetMaterial())
                {
                    DialogueData dialogueData = ResourceManager.Instance.GetDialogueDataByType(DialogueType.RELIC_COLLECT);
                    System.Action callback = () =>
                    {
                        LevelManager.Instance.OnInitLevelByID(_presenterData.Level + 1);
                        Hide();
                    };

                    DialogueManager.Instance.ShowDialougeBox(dialogueData, callback);
                    return;
                }
            }
        }

        Hide();
    }

    private void UpdateTxtMaterial(int mal)
    {
        _TextMaterial.text = "+" + mal;
    }
    private void UpdateTxtCoin(int coin)
    {
        _TextCoin.text = "+" + coin;
    }
}
