using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class T_ColorHexaDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image BoderImg;
    public TMP_Text LocationTxt;
    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Transform ParentBeforeDrag;

    private Image _colorHexa;
    private T_HexaInBoardData _dataBefore;
    private Transform _parent;
    private string _colorId;

    private void Awake()
    {
        if(this.BoderImg != null)
            this.BoderImg.gameObject.SetActive(false);
    }

    public void Init(T_HexaInBoardData hexaData)
    {
        this._colorHexa = this.GetComponent<Image>();
        this._colorHexa.color = T_Utils.ConvertToColor(hexaData.ColorHexa);
        this.LocationTxt.text = hexaData.Id.ToString();
    }

    public void InitColor(string colorId)
    {
        this._colorId = colorId;
        this._colorHexa = this.GetComponent<Image>();
        this._colorHexa.color = T_Utils.ConvertToColor(colorId);
        this.LocationTxt.enabled = false;
    }

    public void SetEnableBorder(bool isActive)
    {
        this.BoderImg.gameObject.SetActive(isActive);
    }

    public void ChangeColorHexa(int idColor)
    {
        T_HexaButton hexaButton = this.transform.parent.GetComponent<T_HexaButton>();
        T_HexaInBoardData hexaData = hexaButton.GetHexaData();
        string color = T_ConfigValue.ColorList[idColor];
        if (hexaData != null)
            hexaData.ColorHexa = color;
        this._colorHexa.color = T_Utils.ConvertToColor(hexaData.ColorHexa);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this._parent = this.transform.parent;
        T_HexaButton hexaButton = this.transform.parent.GetComponent<T_HexaButton>();
        this._dataBefore = hexaButton.GetHexaData();
        this.ParentAfterDrag = transform.parent;
        this.ParentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        this.SetRayCastTarget(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int count = T_ColumnHexa.Instance.Content.transform.childCount;
        if (count == 0)
        {
            GameObject gObj = Instantiate(T_ColumnHexa.Instance.HexaButton, T_ColumnHexa.Instance.Content.transform);
            gObj.name = string.Format("{0}_{1}", "HexaButton", count + 1);
            T_HexaButton hexaButton = gObj.GetComponent<T_HexaButton>();
            T_HexaInBoardData hexaData = new T_HexaInBoardData();
            hexaData.Id = count + 1;
            hexaData.ColorHexa = this.GetColorId();
            hexaButton.Init(hexaData);
            T_ColumnHexa.Instance.AddDataToObj(hexaButton);
        }

        if(this._dataBefore != null)
        {
            transform.SetParent(ParentAfterDrag);
            this.SetRayCastTarget(true);
            T_HexaButton hexaButton = this.transform.parent.GetComponent<T_HexaButton>();
            hexaButton.ColorHexa = this.transform.GetChild(0).gameObject;
            hexaButton.SetColorHexaInHexaBtn(hexaButton.gameObject);
        }
        else
        {
            transform.SetParent(ParentBeforeDrag);
            this.SetRayCastTarget(true);
        }
    }

    public void SetRayCastTarget(bool raycastTarget)
    {
        Image item = this.GetComponent<Image>();
        item.raycastTarget = raycastTarget;
        this.BoderImg.raycastTarget = raycastTarget;
        this.LocationTxt.raycastTarget = raycastTarget;
    }

    public T_HexaInBoardData GetDataBefore()
    {
        return this._dataBefore;
    }

    public string GetColorId()
    {
        return this._colorId;
    }

    public Transform GetParentBefore()
    {
        return this._parent;
    }
}
