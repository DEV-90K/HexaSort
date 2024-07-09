using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class T_HexaButton : MonoBehaviour, IDropHandler
{
    public GameObject ColorHexa;
    private T_HexaInBoardData _hexaData;
    private T_ColorHexaDrag _colorHexa;

    public void Init(T_HexaInBoardData hexaData)
    {
        this._colorHexa = this.ColorHexa.GetComponent<T_ColorHexaDrag>();
        this._hexaData = hexaData;
        this._colorHexa.Init(this._hexaData);
    }

    public void SetHexaData(T_HexaInBoardData hexaData)
    {
        this._hexaData = hexaData;
    }

    public T_HexaInBoardData GetHexaData()
    {
        return this._hexaData;
    }

    public T_ColorHexaDrag GetColorHexa()
    {
        return this._colorHexa;
    }

    public void SetColorHexa(T_ColorHexaDrag colorHexa)
    {
        this._colorHexa = colorHexa;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        T_ColorHexaDrag draggableItem = dropped.GetComponent<T_ColorHexaDrag>();
        if (this.transform.childCount > 0)
        {
            GameObject current = this.transform.GetChild(0).gameObject;

            T_ColorHexaDrag dragCurrent = current.GetComponent<T_ColorHexaDrag>();
            dragCurrent.transform.SetParent(draggableItem.ParentAfterDrag);
            draggableItem.ParentAfterDrag = this.transform;

            GameObject gObj = current.transform.parent.gameObject;
            T_HexaButton hexaButton = gObj.GetComponent<T_HexaButton>();
            this.SetColorHexaInHexaBtn(gObj);
            this.ChangeData(hexaButton, this, T_ScreenTool.Instance.GetHexaObj());
        }
    }

    public void SetColorHexaInHexaBtn(GameObject current)
    {
        T_HexaButton hexaButton = current.GetComponent<T_HexaButton>();
        hexaButton.ColorHexa = current.transform.GetChild(0).gameObject;
    }

    public void ChangeData(T_HexaButton hexa_1, T_HexaButton hexa_2, T_HexaInBoardObject hexaObj)
    {
        T_HexaInBoardData data_1 = hexa_1.GetHexaData();
        hexa_1.SetHexaData(hexa_2.GetHexaData());
        hexa_2.SetHexaData(data_1);

        /*T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
        Debug.LogError(hexaObj.gameObject);
        hexaData.HexagonDatas[hexa_1.GetHexaData().Id - 1] = hexa_1.GetHexaData();
        hexaData.HexagonDatas[hexa_2.GetHexaData().Id - 1] = hexa_2.GetHexaData();*/
    }

    public void OnHexaButtonClick(GameObject gObj)
    {
        T_ColorHexaDrag colorHexa = gObj.GetComponentInChildren<T_ColorHexaDrag>();
        T_ScreenTool.Instance.SetSelectedColorHexa(colorHexa);
    }
}
