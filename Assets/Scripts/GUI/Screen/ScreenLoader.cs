using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtPrecent;
    [SerializeField]
    private Slider loadingBar;

    private float targetProgress = 0;
    private float fillSpeed = 0.015f;
    private float speed = 5f;
    private bool _isLoading = true;

    private void Start()
    {
        _isLoading = true;
        targetProgress = 0;
        UpdateLoadingBar(0f);
        UpdateTextPercent(0f);
    }

    public void LoadToTarget(float? target = null)
    {
        if(target == null)
        {
            target = Random.Range(loadingBar.value, 99f);
        }

        targetProgress = (float)target;
    }

    public void LoadToComplete(float timeRemain = 0f)
    {
        targetProgress = 100;
        StartCoroutine(IE_Remaning(timeRemain));
    }

    private IEnumerator IE_Remaning(float time)
    {
        yield return new WaitForSeconds(time);
        _isLoading = false;        
        yield return IE_OnLoading(loadingBar.maxValue);
    }

    private IEnumerator IE_OnLoading(float target)
    {
        float currVal = loadingBar.value;
        float t = 0;

        while (t < 1)
        {
            t += speed * Time.deltaTime;
            float value = Mathf.Lerp(currVal, target, t);
            UpdateLoadingBar(value);
            UpdateTextPercent((float)value);
            yield return new WaitForEndOfFrame();
        }

        UpdateLoadingBar((float) target);
        UpdateTextPercent((float) target);
    }

    private void Update()
    {
        if(_isLoading == false)
        {            
            return;
        }

        float currentFillAmount = loadingBar.value;
        float progressDifference = Mathf.Abs(currentFillAmount - targetProgress);
        float dynamicFillSpeed = progressDifference * fillSpeed;
        float value = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);

        UpdateLoadingBar(value);
        UpdateTextPercent(value);
    }

    private void UpdateTextPercent(float percent)
    {
        txtPrecent.text = Mathf.CeilToInt(percent) + "%";
    }

    private void UpdateLoadingBar(float value)
    {
        loadingBar.value = value;
    }

    public bool CheckLoad()
    {
        return loadingBar.value == loadingBar.maxValue;
    }
}
