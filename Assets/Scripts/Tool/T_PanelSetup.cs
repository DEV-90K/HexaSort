using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class T_PanelSetup : T_PanelBase
{
    public TMP_InputField NumberHexaInBoardTxt;
    public TMP_InputField NumberColorTxt;

    private int NumberHexaInBoard = 5;
    private int NumberColor = 8;

    private void OnEnable()
    {
        this.NumberHexaInBoardTxt.text = this.NumberHexaInBoard.ToString();
        this.NumberColorTxt.text = this.NumberColor.ToString();
    }
    public void OnConfirmBtnClick()
    {
        int numberHexa = int.Parse(this.NumberHexaInBoardTxt.text);
        int numberColor = int.Parse(this.NumberColorTxt.text);

        T_ScreenTool.Instance.InitLevel(numberHexa, numberColor);
        T_GameController.Instance.ShowGrid();
        //T_GridController.Instance.Init(numberHexa);

        T_GridController.Instance.InitChallenge(10, 6);
        this.Hide();
        T_GridController.Instance.CanContact = true;
    }
    public void OnCloseBtnClick()
    {
        this.Hide();
        T_GridController.Instance.CanContact = true;
    }
}