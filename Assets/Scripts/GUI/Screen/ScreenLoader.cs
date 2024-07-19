using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtPrecent;

    private float speed = 5f;
    private float currentValue = 0;

    private void Start()
    {
        UpdateTextPercent(0f);
    }

    public void LoadToTarget(float? target = null)
    {
        if(target == null)
        {
            target = Random.Range(currentValue, 100f);
        }

        StartCoroutine(IE_OnLoading((float)target));
    }

    private IEnumerator IE_OnLoading(float target)
    {        
        float currVal = currentValue;
        float t = 0;

        while(t < 1)
        {
            t += speed * Time.deltaTime;
            float value = Mathf.Lerp(currVal, target, t);
            currentValue = value;
            UpdateTextPercent((float)value);
            yield return null;
        }

        currentValue = target;
        UpdateTextPercent((float)currentValue);
    }


    private void UpdateTextPercent(float percent)
    {
        txtPrecent.text = Mathf.CeilToInt(percent) + "%";
    }

    public bool CheckLoad()
    {
        return currentValue == 100;
    }
}
