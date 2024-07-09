using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class T_PanelSetup : T_PanelBase
{
    public TMP_InputField NumberHexaInBoardTxt;
    public TMP_InputField NumberColorTxt;
    public void OnConfirmBtnClick()
    {
        int numberHexa = int.Parse(this.NumberHexaInBoardTxt.text);
        int numberColor = int.Parse(this.NumberColorTxt.text);

        T_ScreenTool.Instance.InitLevel(numberHexa, numberColor);
        T_GameController.Instance.ShowGrid();
        this.Hide();
    }
    public void OnCloseBtnClick()
    {
        this.Hide();
    }
}
