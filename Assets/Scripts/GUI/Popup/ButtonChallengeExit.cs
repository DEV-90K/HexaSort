using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonChallengeExit : MonoBehaviour
{
    private Button _btnExit;

    private void Awake()
    {
        _btnExit = GetComponent<Button>();
    }

    private void Start()
    {
        _btnExit.onClick.AddListener(OnClickExit);
    }

    private void OnDestroy()
    {
        _btnExit.onClick.RemoveListener(OnClickExit);        
    }

    private void OnClickExit()
    {
        ChallengeManager.Instance.OnExit();
    }
}
