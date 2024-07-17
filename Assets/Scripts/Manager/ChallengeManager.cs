using System;
using UnityEngine;

public class ChallengeManager : MonoSingleton<ChallengeManager>
{
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackManager _stackManager;

    private ChallengeData _challengeData;
    private ChallengePresenterData _presenterData;

    private GalleryRelicData _galleryRelicData;

    private void OnEnable()
    {
        StackMerger.OnStackMergeCompleted += StackMerge_OnStackMergeCompleted;
    }

    private void OnDisable()
    {
        StackMerger.OnStackMergeCompleted -= StackMerge_OnStackMergeCompleted;
    }

    private void StackMerge_OnStackMergeCompleted()
    {

    }

    public void OnInit(GalleryRelicData galleryRelicData)
    {
        _galleryRelicData = galleryRelicData;
        //Base on ID of Relic choice Challenge play
        //ChallengeData challengeData = ResourceManager.instance.GetChallengeByID(_galleryRelicData.IDRelic);
        //ChallengePresenterData challengePresenterData = ResourceManager.instance.GetChallengePresenterDataByID(_galleryRelicData.IDRelic);

        //TEST
        ChallengeData challengeData = ResourceManager.instance.GetChallengeByID(1);
        ChallengePresenterData challengePresenterData = ResourceManager.instance.GetChallengePresenterDataByID(1);

        OnInit(challengeData, challengePresenterData);

        //this.Invoke(() => OnFinishWoned(), 5f);
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
        this.Invoke(() => HanldeFinish(), 1f);
    }    

    private void HanldeFinish()
    {
        //TODO: Add material base presenter data
        //TODO: Add coin base presenter data

        MainPlayer.instance.CollectGalleryRelic(_galleryRelicData);
        GUIManager.instance.ShowScreen<ScreenMain>();
    }

    #region Screen Challenge
    internal void ShowStackLeft()
    {
        throw new NotImplementedException();
    }

    internal bool CanShowLeft()
    {
        throw new NotImplementedException();
    }

    internal void ShowStackRight()
    {
        throw new NotImplementedException();
    }

    internal bool CanShowRight()
    {
        throw new NotImplementedException();
    }
    #endregion Screen Challenge
}
