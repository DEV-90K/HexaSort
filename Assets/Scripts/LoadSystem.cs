using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSystem : MonoBehaviour
{    
    [SerializeField]
    private float _TimeLoading = 5f;
    [SerializeField]
    private ScreenLoader _ScreenLoader;

    private TimerUtils.CountdownTimer _CountDownTimer = null;

    private void OnEnable()
    {
        GameManager.OnChangeState += GameManager_OnChangeState;
    }

    private void OnDisable()
    {
        GameManager.OnChangeState -= GameManager_OnChangeState;
    }

    private void GameManager_OnChangeState(GameState state)
    {
        if(state == GameState.LOADING)
        {
            _CountDownTimer.Start();
        }
    }

    private void Start()
    {        
        _CountDownTimer = new TimerUtils.CountdownTimer(_TimeLoading);
        _CountDownTimer.OnTimerStart += EnterLoading;
        _CountDownTimer.OnTimerStop += ExitLoading;        
    }

    private void Update()
    {
        if(_CountDownTimer != null)
            _CountDownTimer.Tick(Time.deltaTime);
    }

    private async void EnterLoading()
    {
        Debug.Log("Load System");
        await FirebaseConnecting();
        Debug.Log("FirebaseConnecting Completed");
        _ScreenLoader.LoadToTarget(null);
        await ResourceLoading();
        Debug.Log("ResourceLoading Completed");
        _ScreenLoader.LoadToTarget(null);
        await PlayerLoading();
        _ScreenLoader.LoadToTarget(100f);
        _CountDownTimer.Stop();
    }

    private void ExitLoading()
    {
        StartCoroutine(IE_SceneLoading("Game"));
        Debug.Log("IE_SceneLoading Completed");
    }

    private async Task FirebaseConnecting()
    {
        int retry = 3;

        while (true)
        {
            try
            {
                await FirebaseManager.Instance.ConnectToFirebase();
                Debug.Log("Return FirebaseTask");
                return;
            }
            catch (Exception e)
            {
                Debug.Log("Catch FirebaseTask: " + e.ToString());
                if (retry == 0)
                {
                    return;
                }

                await Task.Delay(retry * 1000 + UnityEngine.Random.Range(0, retry * 1000));
                retry--;
            }
        }
    }

    private async Task ResourceLoading()
    {
        ResourceManager.Instance.LoadResource();
        await Task.CompletedTask;
    }

    private async Task PlayerLoading()
    {
        MainPlayer.Instance.LoadData();
        await Task.CompletedTask;
    }

    private IEnumerator IE_SceneLoading(string sceneName)
    {
        yield return new WaitUntil(() => _ScreenLoader.CheckLoad());

        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncOp.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(currentSceneName);
        GameManager.Instance.ChangeState(GameState.START);
    }
}
