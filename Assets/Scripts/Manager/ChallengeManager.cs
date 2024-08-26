using System;
using System.Collections;
using UnityEngine;

public class ChallengeManager : MonoSingleton<ChallengeManager>
{
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackChallengeManager _stackManager;

    private ChallengeData _challengeData;
    private ChallengePresenterData _presenterData;
    private GalleryRelicData _galleryRelicData;

    private void Start()
    {
        ChallengeController.OnTurnCompleted += ChallengeController_OnTurnCompleted;
    }

    private void OnDestroy()
    {
        ChallengeController.OnTurnCompleted -= ChallengeController_OnTurnCompleted;
    }

    private void ChallengeController_OnTurnCompleted()
    {
        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonContainStack();
        if(gridHexagons.Length == 0)
        {
            OnFinishWoned();
            
        }
        else
        {
            StackHexagon[] stackHexagons = _stackManager.GetStackHexagonsCanPlace();
            if(stackHexagons.Length == 0)
            {
                OnFinishLosed();
            }
        }
    }

    public void OnInit(GalleryRelicData galleryRelicData)
    {
        _galleryRelicData = galleryRelicData;
        //Base on ID of Relic choice Challenge play
        //ChallengeData challengeData = ResourceManager.instance.GetChallengeByID(_galleryRelicData.IDRelic);
        //ChallengePresenterData challengePresenterData = ResourceManager.instance.GetChallengePresenterDataByID(_galleryRelicData.IDRelic);

        //TEST
        ChallengeData challengeData = ResourceManager.instance.GetChallengeByID(2);
        ChallengePresenterData challengePresenterData = ResourceManager.instance.GetChallengePresenterDataByID(2);

        OnInit(challengeData, challengePresenterData);
    }

    public void OnInit(ChallengeData challengeData, ChallengePresenterData presenterData)
    {
        _challengeData = challengeData;
        _presenterData = presenterData;

        _gridManager.OnInit(_challengeData.Grid);
        _stackManager.OnInit(_challengeData.StackQueueData);

        GUIManager.instance.ShowScreen<ScreenChallenge>(_presenterData);
    }

    private void OnInitCurrentChallenge()
    {
        OnInit(_challengeData, _presenterData);
    }

    public void OnReplay()
    {
        _stackManager.CollectRandomImmediate();
        _gridManager.CollectGridImmediate();
        OnInitCurrentChallenge();
    }

    public void OnExit()
    {
        _stackManager.CollectRandomImmediate();
        _gridManager.CollectGridImmediate();

        MainPlayer.instance.CollectGalleryRelic(_galleryRelicData);        
        GUIManager.instance.ShowScreen<ScreenMain>();
    }

    private void OnFinish()
    {
        GameManager.instance.ChangeState(GameState.FINISH);
        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();
    }

    private void OnFinishWoned()
    {
        OnFinish();
        StartCoroutine(IE_FinishWoned(1f));
    }    

    private IEnumerator IE_FinishWoned(float delay)
    {
        yield return new WaitForSeconds(delay);

        _galleryRelicData.State = GalleryRelicState.COLLECT;
        MainPlayer.instance.CollectGalleryRelic(_galleryRelicData);
        //GUIManager.instance.ShowPopup<PopupChallengeWoned>(_galleryRelicData);
    }

    public void OnFinishLosed()
    {
        OnFinish();
        StartCoroutine(IE_FinishLosed(1f));
    }

    private IEnumerator IE_FinishLosed(float delay)
    {
        yield return new WaitForSeconds(delay);

        _galleryRelicData.State = GalleryRelicState.LOCK;
        MainPlayer.instance.CollectGalleryRelic(_galleryRelicData);
        //GUIManager.instance.ShowPopup<PopupChallengeLosed>(_galleryRelicData);
    }


    #region Screen Challenge
    internal void ShowStackLeft()
    {
        _stackManager.ShowStackLeft();
    }

    internal bool CanShowLeft()
    {
        return _stackManager.CanShowLeft();
    }

    internal void ShowStackRight()
    {
        _stackManager.ShowStackRight();
    }

    internal bool CanShowRight()
    {
        return _stackManager.CanShowRight();
    }
    #endregion Screen Challenge
}
