using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBoostHammer : MonoBehaviour
{
    [SerializeField]
    private GameObject _ObjNoti;
    [SerializeField]
    private TMP_Text _txtAmount;

    private Button mButton;
    private IBoostHammer _able;

    private int _amount;

    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        MainPlayer.OnChangeHammer += MainPlayer_OnChangeHammer;
    }

    private void OnDestroy()
    {
        MainPlayer.OnChangeHammer -= MainPlayer_OnChangeHammer;
    }

    private void MainPlayer_OnChangeHammer(int amount)
    {
        if (amount > 0)
        {
            _ObjNoti.SetActive(true);
            mButton.interactable = true;
            UpdateTxtAmount(amount);
        }
        else
        {
            mButton.interactable = false;
            _ObjNoti.SetActive(false);
        }
    }

    public void OnInit(IBoostHammer able)
    {
        _able = able;
        int amount = MainPlayer.Instance.GetHammer();
        MainPlayer_OnChangeHammer(amount);
    }

    private void UpdateTxtAmount(int amount)
    {
        _txtAmount.text = amount.ToString();
    }

    private void OnClickButton()
    {
        if(GameManager.Instance.IsState(GameState.PAUSE))
        {
            return;
        }

        GUIManager.Instance.HideScreen<ScreenLevel>();
        GUIManager.Instance.ShowPopup<PopupBoostHammer>(_able);
    }
}
