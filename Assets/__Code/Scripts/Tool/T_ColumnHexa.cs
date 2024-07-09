using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class T_ColumnHexa : MonoBehaviour
{
    public static T_ColumnHexa Instance;
    public GameObject Content;
    public GameObject HexaButton;

    private T_HexaInBoardObject _hexaObject;
    private List<GameObject> _childs;
    private GameObject _hexaButton;
    private GameObject _hexaBtnSelected;
    private void Awake()
    {
        Instance = this;

        this._childs = new List<GameObject>();
        //this._hexaButton = this.Content.transform.GetChild(0).gameObject;
    }
    public void ShowColumnHexa(T_HexaInBoardObject hexa)
    {
        this._hexaObject = hexa;
        T_HexaInBoardData hexaData = hexa.GetDataHexa();
        this.DeleteChildInContent(this._childs);

        /*int childCount = this.Content.transform.childCount;
        int count = childCount;
        if(childCount > hexaData.HexagonDatas.Length)
        {
            this.DeleteChildInContent(this._childs);
        };
        for(int i = 0; i < hexaData.HexagonDatas.Length; i++)
        {
            GameObject gObj;
            if(childCount > i)
            {
                gObj = this.Content.transform.GetChild(i).gameObject;
                gObj.SetActive(true);
            }
            else
                gObj = Instantiate(this._hexaButton, this.Content.transform);
            gObj.name = string.Format("{0}_{1}", "HexaButton", i + 1);
            T_HexaButton hexaBtn = gObj.GetComponent<T_HexaButton>();
            hexaBtn.Init(hexaData.HexagonDatas[i]);
            this._childs.Add(gObj);
        }*/

        for (int i = 0; i < hexaData.HexagonDatas.Length; i++)
        {
            GameObject gObj = Instantiate(this.HexaButton, this.Content.transform);
            gObj.name = string.Format("{0}_{1}", "HexaButton", i + 1);
            T_HexaButton hexaBtn = gObj.GetComponent<T_HexaButton>();
            hexaBtn.Init(hexaData.HexagonDatas[i]);
            this._childs.Add(gObj);
        }
    }

    public void SetUpHexaObj(T_HexaInBoardObject hexaObj)
    {
        T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
        for (int i = 0; i < hexaData.HexagonDatas.Length; i++)
        {
            T_HexaButton hexaBtn = this.Content.transform.GetChild(i).GetComponent<T_HexaButton>();
            hexaData.HexagonDatas[i] = hexaBtn.GetHexaData();
        }
    }

    public void DeleteChildInContent(List<GameObject> childs)
    {
        if(childs.Count > 0)
        {
            foreach (GameObject child in childs)
            {
                Destroy(child);
            }
        }
    }

    public void OnHexaBtnClick(GameObject gObj)
    {
        T_ColorButton colorButton = gObj.GetComponent<T_ColorButton>();
        T_ScreenTool.Instance.SelectedHexaInColumn(colorButton);
    }

    public void OnHexaButtonClick(GameObject gObj)
    {
        T_ColorHexaDrag colorHexa = gObj.GetComponentInChildren<T_ColorHexaDrag>();
        T_ScreenTool.Instance.SetSelectedColorHexa(colorHexa);
    }

    public void OnCloneBtnClick()
    {
        T_HexaInBoardData dataObj = this._hexaObject.GetDataHexa();
        int count = dataObj.HexagonDatas.Length;

        T_HexaInBoardData[] dataObjNew = new T_HexaInBoardData[count + 1];
        for(int i = 0; i < count; i++)
        {
            dataObjNew[i] = dataObj.HexagonDatas[i];
        }

        this._hexaBtnSelected = T_ScreenTool.Instance.GetColorHexa().transform.parent.gameObject;
        if(this._hexaBtnSelected != null)
        {
            T_HexaInBoardData hexaData = this._hexaBtnSelected.GetComponent<T_HexaButton>().GetHexaData();
            GameObject gObj = Instantiate(this._hexaBtnSelected, this.Content.transform);
            gObj.SetActive(true);
            gObj.name = string.Format("{0}_{1}", "HexaButton", count + 1);
            T_HexaButton hexaButton = gObj.GetComponent<T_HexaButton>();
            hexaButton.Init(hexaData);
            this._childs.Add(gObj);
            dataObjNew[count] = hexaData;
            this._hexaObject.GetDataHexa().HexagonDatas = dataObjNew;
        }
    }

    public void OnRemoveBtnClick()
    {
        this._hexaBtnSelected = T_ScreenTool.Instance.GetColorHexa().transform.parent.gameObject;
        if (this._hexaBtnSelected != null)
        {
            T_HexaInBoardData hexaData = this._hexaBtnSelected.GetComponent<T_HexaButton>().GetHexaData();
            T_HexaInBoardData[] array = this._hexaObject.GetDataHexa().HexagonDatas;
            array = array.Where(s => s.Id != hexaData.Id).ToArray();
            this._hexaObject.GetDataHexa().HexagonDatas = array;
            Destroy(this._hexaBtnSelected);
        }
    }
}
