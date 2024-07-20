using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBoostHammer : MonoBehaviour
{
    private Button mButton;
    private IBoostHammer _able;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    public void OnInit(IBoostHammer able)
    {
        _able = able;
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
