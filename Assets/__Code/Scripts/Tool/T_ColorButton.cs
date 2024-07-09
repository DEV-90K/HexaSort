using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class T_ColorButton : MonoBehaviour, IDropHandler
{
    public GameObject SeletedBoder;
    public TMP_Text LocationTxt;
    public GameObject ColorHexa;

    private T_HexaInBoardData _hexaData;
    private Image _imageColorHexa;
    private int _locationId;
    private string _color;
    private void Awake()
    {
        if(this.SeletedBoder != null)
            this.SeletedBoder.SetActive(false);

        if(this.ColorHexa != null)
            this._imageColorHexa = this.ColorHexa.GetComponent<Image>();
    }

    public void InitColor(int locationId = -1, bool changeLocation = true)
    {
        string color = locationId < 0 ? "" : T_ConfigValue.ColorList[locationId];
        this._color = color;
        this._imageColorHexa.color = T_Utils.ConvertToColor(color);
        if (!changeLocation) return;
        this._locationId = locationId;
        this.LocationTxt.text = (locationId + 1).ToString();
    }

    public void InitHexaColumn(T_HexaInBoardData hexaData, int location)
    {
        this._hexaData = hexaData;
        this._imageColorHexa.color = T_Utils.ConvertToColor(hexaData.ColorHexa);
        this.LocationTxt.text = (location + 1).ToString();
    }
    
    public void ChangeColorHexa(int idColor)
    {
        string color = T_ConfigValue.ColorList[idColor];
        if (this._hexaData != null)
            this._hexaData.ColorHexa = color;
        this._imageColorHexa.color = T_Utils.ConvertToColor(this._hexaData.ColorHexa);
    }

    public void SetSelected(bool isSelected)
    {
        if (this.SeletedBoder != null)
            this.SeletedBoder.SetActive(isSelected);
    }

    public int GetId()
    {
        return this._locationId;
    }

    public string GetColorHexa()
    {
        return this._color;
    }

    public T_HexaInBoardData GetHexaData()
    {
        return this._hexaData;
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

            //Debug.Log(string.Format("{0}_{1}", this.gameObject, this._hexaData.colorHexa.ToString()));
        }
    }
}
