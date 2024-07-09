using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_ConfigValue
{
    public static string[] ColorList = new string[12]
    {
      "#cc0022",
      "#014f41",
      "#d29bc2",
      "#990913",
      "#f4e7de",
      "#6ba1bc",
      "#256832",
      "#871616",
      "#87694d",
      "#dfdbdb",
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

    public T_PanelSetup PanelSetup;
    public T_PanelColorGroup PanelColorGroup;
    public T_ColumnHexa ColumnHexa;

    private T_HexaInBoardObject _hexaObj;
    private int _hexInEachHexaNumber;
    private int _colorNumber;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.HideOnClickHexaDisable();
        this.PanelSetup.Hide();
    }

    public void InitLevel(int hexInEachHexaNumber, int colorNumber)
    {
        this._hexInEachHexaNumber = hexInEachHexaNumber;
        this._colorNumber = colorNumber;
    }
    public void OnRemoveBtnClick()
    {
        if (this._hexaObj == null) return;
        T_GridController.Instance.ShowEmptyHexa();
        this._hexaObj.SetSelectedHexa(false);
        this.HideOnClickHexaDisable();
    }

    public void OnShowBtnClick()
    {
        if (this._hexaObj == null) return;
        T_ColumnHexa.Instance.SetUpHexaObj(this._hexaObj);
        T_GridController.Instance.ShowNumberHexaInHexa(this._hexaObj);
        this.HideOnClickHexaDisable();
    }

    public void OnEmptyBtnClick()
    {
        if (this._hexaObj == null) return;
        T_GridController.Instance.ShowEmptyHexa();
        this.HideOnClickHexaDisable();
    }

    public void ShowOnClickHexa(T_HexaInBoardObject hexaObj, bool isEnable)
    {
        if (!isEnable)
        {
            this._hexaObj = null;
            this.HideOnClickHexaDisable();
        }
        else
        {
            this._hexaObj = hexaObj;
            this.ShowOnClickHexaEnable();
        }
    }

    public void OnSetupBtnClick()
    {
        this.PanelSetup.Show();
    }

    public void ShowOnClickHexaEnable()
    {
        this.RemoveBtn.SetActive(true);
        this.ShowBtn.SetActive(true);
        this.EmptyBtn.SetActive(true);
        this.ColumnHexa.gameObject.SetActive(true);
        this.ColumnHexa.ShowColumnHexa(this._hexaObj);
        //this.PanelColorGroup.Show();
        //this.PanelColorGroup.InitColorBtn(_colorNumber);
    }

    public void HideOnClickHexaDisable()
    {
        this.RemoveBtn.SetActive(false);
        this.ShowBtn.SetActive(false);
        this.EmptyBtn.SetActive(false);
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

    private T_ColorHexaDrag _colorHexaDrag;
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
        if (this._colorBtnSelected == null) return;
        this._colorBtnSelected.ChangeColorHexa(idColor);
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

    public T_HexaInBoardObject GetHexaObj()
    {
        return this._hexaObj;
    }

    public T_ColorHexaDrag GetColorHexa()
    {
        return this._colorHexaDrag;
    }
}
