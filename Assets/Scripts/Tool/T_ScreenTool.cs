using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class T_ConfigValue
{
    public static string[] ColorList = new string[12]
    {
      "#cc0022",
      "#FFB200",
      "#d29bc2",
      "#5BBCFF",
      "#D10363",
      "#FFFF80",
      "#256832",
      "#871616",
      "#87694d",
      "#131842",
      "#964CFA",
      "#6266F9",
    };
}

public class T_ScreenTool : MonoBehaviour
{
    public static T_ScreenTool Instance;

    public GameObject RemoveBtn;
    public GameObject ShowBtn;
    public GameObject EmptyBtn;
    public GameObject HideBtn;
    public GameObject StackBtn;

    public T_PanelSetup PanelSetup;
    public T_PanelColorGroup PanelColorGroup;
    public T_PanelExport PanelExport;
    public T_PanelCompress PanelCompress;
    public T_ColumnHexa ColumnHexa;

    private T_HexaInBoardObject _hexaObj;
    private int _hexInEachHexaNumber;
    private int _colorNumber;
    private bool _isChallenge;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.HideOnClickHexaDisable();
        this.PanelSetup.Hide();
        this.PanelExport.Hide();
        this.PanelCompress.Hide();

        this._isChallenge = false;
    }

    public void InitLevel(int hexInEachHexaNumber, int colorNumber)
    {
        this._hexInEachHexaNumber = hexInEachHexaNumber;
        this._colorNumber = colorNumber;
    }
    public void OnRemoveBtnClick()
    {
        this._hexaObj = T_GridController.Instance.GetHexaObjSelected();
        if (this._hexaObj == null) return;
        T_GridController.Instance.ShowEmptyHexa(this._hexaObj);
        this._hexaObj.SetVisualState(VisualState.EMPTY);
        this._hexaObj.SetSelectedHexa(false);
        this.HideOnClickHexaDisable();
    }

    public void OnShowBtnClick()
    {
        this._hexaObj = T_GridController.Instance.GetHexaObjSelected();
        if (this._hexaObj == null) return;
        T_ColumnHexa.Instance.SetUpHexaObj(this._hexaObj);
        this._hexaObj.SetVisualState(VisualState.SHOW);
        T_GridController.Instance.ShowNumberHexaInHexa(this._hexaObj);
        this.HideOnClickHexaDisable();
    }

    public void OnEmptyBtnClick()
    {
        this._hexaObj = T_GridController.Instance.GetHexaObjSelected();
        if (this._hexaObj == null) return;
        T_GridController.Instance.ShowEmptyHexa(this._hexaObj);
        this._hexaObj.Init(0);
        this._hexaObj.SetVisualState(VisualState.EMPTY);
        this.HideOnClickHexaDisable();
    }

    public void OnHideBtnClick()
    {
        this._hexaObj = T_GridController.Instance.GetHexaObjSelected();
        if (this._hexaObj == null) return;
        //this._hexaObj.Init(0);
        T_GridController.Instance.ShowEmptyHexa(this._hexaObj);
        this._hexaObj.SetVisualState(VisualState.HIDE);
        this.HideOnClickHexaDisable();
    }

    public void OnExportBtnClick()
    {
        this.PanelExport.Show();
        T_GridController.Instance.CanContact = false;
    }

    public void OnCompressBtnClick()
    {
        this.PanelCompress.Show();
    }

    public void ShowOnClickHexa(T_HexaInBoardObject hexaObj, bool isEnable)
    {
        if (!isEnable)
        {
            T_GridController.Instance.SetHexaObjSeleted(null);
            //this._hexaObj = null;
            this.HideOnClickHexaDisable();
        }
        else
        {
            T_GridController.Instance.SetHexaObjSeleted(hexaObj);
            //this._hexaObj = hexaObj;
            this.ShowOnClickHexaEnable();
        }
    }

    public void OnSetupBtnClick()
    {
        this.PanelSetup.Show();
        T_GridController.Instance.CanContact = false;
    }

    public void ShowOnClickHexaEnable()
    {
        this._hexaObj = T_GridController.Instance.GetHexaObjSelected();
        this.RemoveBtn.SetActive(true);
        this.ShowBtn.SetActive(true);
        this.EmptyBtn.SetActive(true);
        this.HideBtn.SetActive(true);
        this.StackBtn.SetActive(true);
        this.ColumnHexa.gameObject.SetActive(true);
        this.ColumnHexa.ShowColumnHexa(this._hexaObj);
        this.PanelColorGroup.Show();
        //this.PanelColorGroup.InitColorBtn(_colorNumber);
        this.PanelColorGroup.InitColor(_colorNumber);
    }

    public void HideOnClickHexaDisable()
    {
        this.RemoveBtn.SetActive(false);
        this.ShowBtn.SetActive(false);
        this.EmptyBtn.SetActive(false);
        this.HideBtn.SetActive(false);
        this.StackBtn.SetActive(false);
        this.ColumnHexa.gameObject.SetActive(false);
        this.PanelColorGroup.Hide();
    }

    private T_ColorButton _colorBtnSelected;
    public void SelectedHexaInColumn(T_ColorButton hexaBtn)
    {
        if(this._colorBtnSelected != null)
        {
            this._colorBtnSelected.SetSelected(false);
            if(this._colorBtnSelected == hexaBtn)
            {
                this._colorBtnSelected = null;
                return;
            }
        }

        this._colorBtnSelected = hexaBtn;
        this._colorBtnSelected.SetSelected(true);
    }

    private T_ColorHexaDrag _colorHexaDrag = null;
    public void SetSelectedColorHexa(T_ColorHexaDrag colorHexaDrag)
    {
        if (this._colorHexaDrag != null)
        {
            this._colorHexaDrag.SetEnableBorder(false);
            if (this._colorHexaDrag == colorHexaDrag)
            {
                this._colorHexaDrag = null;
                return;
            }
        }

        this._colorHexaDrag = colorHexaDrag;
        this._colorHexaDrag.SetEnableBorder(true);
    }

    public void SetColorHexa(int idColor)
    {
        if (this._colorHexaDrag == null) return;
        this._colorHexaDrag.ChangeColorHexa(idColor);
        //this._colorBtnSelected.InitColor(idColor, false);
    }

    public int GetColorNumber()
    {
        return this._colorNumber;
    }

    public int GetHexaInEachHexaNumber()
    {
        return this._hexInEachHexaNumber;
    }

    public T_ColorHexaDrag GetColorHexa()
    {
        if (this._colorHexaDrag == null) return null;
        return this._colorHexaDrag;
    }

    public void OnDemoBtnClick()
    {
        //T_GUIManager.Instance.ShowDemo();
        LevelData level = T_GridController.Instance.GetLevelData();
        //T_LevelManager.Instance.LoadLevelByData(level);
        StackQueueData queueData = T_GridController.Instance.GetStackQueueData();
        ChallengeData challengeData = new ChallengeData(level.Grid, queueData);
        ChallengePresenterData challengePresenterData = new ChallengePresenterData(1);

        T_Data.Instance._challengeData = challengeData;
        T_Data.Instance._presenterData = challengePresenterData;
        T_Data.Instance._hexasSelected = T_GridController.Instance.GetHexaObjsSelectedData();
        T_LevelManager.Instance.CacheData();
        //T_LevelManager.Instance.SetChallengeData(challengeData, challengePresenterData);
        //T_LevelManager.Instance.SetHexaSelected(T_GridController.Instance.GetHexaObjsSelected());
        SceneManager.LoadScene("Game");
  
        
    }

    public void OnChallengeBtnClick()
    {
        this._isChallenge = !this._isChallenge;
        /*if (this._isChallenge)
        {
            T_GridController.Instance.SetUpChallenge();
        }*/
        T_GridController.Instance.SetUpChallenge();

    }

    public void OnStackBtnClick()
    {
        /*if(this._isChallenge)
        {
        }*/
            T_GridController.Instance.SetStackHexa(this._hexaObj);
    }

    /*public T_LevelData GetTLevelData()
    {
        List<T_HexaInBoardObject> hexaObjSelected = T_GridController.Instance.GetHexaObjsSelected();
        int count = hexaObjSelected.Count;
        T_LevelData result = new T_LevelData();
        result.Level = 0;
        result.HexaInBoardDatas = new T_HexaInBoardData[count];
        if(count > 0)
        {
            for(int i = 0; i < count; i++)
            {
                T_HexaInBoardObject hexaObj = hexaObjSelected[i];
                T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
                if (hexaData.HexagonDatas.Length > hexaObj.transform.childCount) hexaData.HexagonDatas = new T_HexaInBoardData[0];
                result.HexaInBoardDatas[i] = hexaData;
            }
        }
        return result;
    }*/
}
