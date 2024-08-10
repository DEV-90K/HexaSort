using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonChest_ScreenMain : MonoBehaviour
{
    [SerializeField]
    private GameObject _Active;

    [SerializeField]
    private GameObject _Deactive;

    private Button _btn;
    protected void Awake()
    {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(OnClickChestReward);
    }

    public void UpdateButton()
    {
        if (MainPlayer.Instance.CheckTimeToChestReward())
        {
            ShowActive();
        }
        else
        {
            ShowDeactive();
        }
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(OnClickChestReward);
    }

    private void ShowActive()
    {        
        _Active.gameObject.SetActive(true);
        _Deactive.gameObject.SetActive(false);

        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClickChestReward);
    }    

    private void OnClickChestReward()
    {
        GUIManager.Instance.ShowPopup<PopupChestReward>(this);
    }

    private void ShowDeactive()
    {
        _Active.gameObject.SetActive(false);
        _Deactive.gameObject.SetActive(true);
        _btn.onClick.RemoveAllListeners();
    }
}
