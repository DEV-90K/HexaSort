using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMain : ScreenBase
{    
    [SerializeField]
    private Button _BtnPlay;
    [SerializeField]
    private Button _BtnGrallery_1;
    [SerializeField]
    private Button _BtnGrallery_2;
    [SerializeField]
    private Button _BtnGrallery_3;
    [SerializeField]
    private TMP_Text _txtCoin;
    [SerializeField]
    private TMP_Text _txtMaterial;
    [SerializeField]
    private Button _btnChestReward;

    private int _coin;
    private int _material;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);

        _coin = MainPlayer.Instance.GetCoin();
        _material = MainPlayer.Instance.GetMaterial();
    }

    public void OnInitWithScene()
    {
        if (MainPlayer.Instance.CheckWelcomePlayer())
        {
            DialogueData data = ResourceManager.Instance.GetDialogueDataByType(DialogueType.WELCOME);
            DialogueManager.Instance.ShowDialougeBox(data);
        }
        else if (MainPlayer.Instance.CheckTimeToChestReward())
        {
            DialogueData data = ResourceManager.Instance.GetDialogueDataByType(DialogueType.CHEST_REWARD);
            DialogueManager.Instance.ShowDialougeBox(data);
        }
        else
        {
            LevelManager.Instance.OnInitCurrentLevel();
            Hide();
        }
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtCoin();
        UpdateTxtMaterial();
    }

    private void OnEnable()
    {
        MainPlayer.OnChangeCoin += MainPlayer_OnChangeCoin;
        MainPlayer.OnChangeMaterial += MainPlayer_OnChangeMaterial;
    }

    private void OnDisable()
    {
        MainPlayer.OnChangeCoin -= MainPlayer_OnChangeCoin;
        MainPlayer.OnChangeMaterial -= MainPlayer_OnChangeMaterial;
    }

    private void MainPlayer_OnChangeCoin(int amount)
    {
        _coin = amount;
        UpdateTxtCoin();
    }    

    private void MainPlayer_OnChangeMaterial(int amount)
    {
        _material = amount;
        UpdateTxtMaterial();
    }    

    private void Start()
    {        
        _BtnPlay.onClick.AddListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.AddListener(OnClickGrallery_1);
        _BtnGrallery_2.onClick.AddListener(OnClickGrallery_2);
        _BtnGrallery_3.onClick.AddListener(OnClickGrallery_3);
        _btnChestReward.onClick.AddListener(OnClickChestReward);
    }

    private void OnDestroy()
    {
        _BtnPlay.onClick.RemoveListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.RemoveListener(OnClickGrallery_1);
        _BtnGrallery_2.onClick.RemoveListener(OnClickGrallery_2);
        _BtnGrallery_3.onClick.RemoveListener(OnClickGrallery_3);
        _btnChestReward.onClick.RemoveListener(OnClickChestReward);
    }

    private void OnClickChestReward()
    {
        GUIManager.Instance.ShowPopup<PopupChestReward>();
    }

    private void OnClickBtnPlay()
    {
        LevelManager.Instance.OnInitCurrentLevel();
        Hide();
    }

    private void OnClickGrallery_1()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(1);
    }

    private void OnClickGrallery_2()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(2);
    }

    private void OnClickGrallery_3()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(3);
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
