using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScaleImage : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        this._image = this.gameObject.GetComponent<Image>();
        Vector2 imgSize = this._image.rectTransform.rect.size;
        imgSize *= (Screen.height / 1920f);
        this._image.rectTransform.sizeDelta = imgSize;
    }

}
