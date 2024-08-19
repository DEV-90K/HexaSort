using Audio_System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMain : ScreenBase
{
    [SerializeField]
    private MusicData musicData;

    [SerializeField]
    private Button _BtnPlay;
    [SerializeField]
    private GUI_ScreenMain.Gallery[] _Galleries; 
    [SerializeField]
    private ButtonChest_ScreenMain _btnChest;

    public void OnInitWithScene()
    {
        if (false && MainPlayer.Instance.CheckWelcomePlayer())
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

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);

        GalleryData[] galleries = ResourceManager.Instance.GetGalleryDatas();

        for(int i = 0; i < _Galleries.Length; i++)
        {
            _Galleries[i].OnInit(galleries[i].ID);
        }
    }

    public override void Show()
    {
        base.Show();
        _btnChest.UpdateButton();

        MusicManager.Instance.Play(musicData);
        GameManager.Instance.ChangeState(GameState.START);
    }


    private void Start()
    {        
        _BtnPlay.onClick.AddListener(OnClickBtnPlay);
    }

    private void OnDestroy()
    {
        _BtnPlay.onClick.RemoveListener(OnClickBtnPlay);
    }

    private void OnClickBtnPlay()
    {
        LevelManager.Instance.OnInitCurrentLevel();
        Hide();
    }
}
