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

    private Image _colorHexa;

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

    public void SetEnableBorder(bool isActive)
    {
        this.BoderImg.gameObject.SetActive(isActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.ParentAfterDrag = transform.parent;
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
        transform.SetParent(ParentAfterDrag);
        this.SetRayCastTarget(true);

        T_HexaButton hexaButton = this.transform.parent.GetComponent<T_HexaButton>();
        hexaButton.ColorHexa = this.transform.GetChild(0).gameObject;
        hexaButton.SetColorHexaInHexaBtn(hexaButton.gameObject);
    }

    public void SetRayCastTarget(bool raycastTarget)
    {
        Image item = this.GetComponent<Image>();
        item.raycastTarget = raycastTarget;
        this.BoderImg.raycastTarget = raycastTarget;
        this.LocationTxt.raycastTarget = raycastTarget;
    }
}
