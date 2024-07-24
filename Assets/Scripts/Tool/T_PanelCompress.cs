using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class T_PanelCompress : T_PanelBase
{
    public TMP_InputField ContentText;
    public TMP_InputField FileNameText;

    public void OnExportBtnClick()
    {
        if (string.IsNullOrEmpty(this.ContentText.text)) return;
        string contentData = CompressText.Compress(this.ContentText.text); 
        string fileName = "CompressFile";
        if(!string.IsNullOrEmpty(this.FileNameText.text))
        {
            fileName = this.FileNameText.text;
        }
        
        Debug.LogError(contentData);
        WebGLFileSaver.SaveFile(contentData, fileName);
    }
}
