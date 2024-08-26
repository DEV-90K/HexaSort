using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupGalleryRelic : PopupBase
{
    [SerializeField]
    private TMP_Text _txtName;
    [SerializeField]
    private TMP_Text _txtDescription;
    [SerializeField]
    private TMP_Text _txtValue;
    [SerializeField]
    private Image _art;

    [SerializeField]
    private Button _btnClose;

    private GalleryRelicData _data;
    private RelicData _relicData;
    public void OnInit(GalleryRelicData inputData)
    {
        _data = inputData;
        _relicData = ResourceManager.Instance.GetRelicDataByID(_data.IDRelic);

        UpdateArtRelic();
        UpdateTxtName();
        _txtDescription.text = "";
        _txtValue.text = "";        
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtDescription();
        UpdateTxtValue();
    }

    private void UpdateTxtName()
    {
        _txtName.text = _relicData.Name;
    }

    private void UpdateTxtDescription()
    {
        //_txtDescription.text = "+ Description: " +_relicData.Description;
        _txtDescription.text = "";
        StartCoroutine(IE_ShowDescription());
    }

    private IEnumerator IE_ShowDescription()
    {
        foreach (char letter in _relicData.Description)
        {
            _txtDescription.text += letter;
            yield return null;
        }
    }

    private void UpdateTxtValue()
    {
        //_txtValue.text = "+ Display value: " +_relicData.Coin + "Coin" + "/" + _relicData.Timer + "Minute";
        _txtValue.text = "";

        StartCoroutine(IE_ShowValue());
    }

    private IEnumerator IE_ShowValue()
    {
        string value = _relicData.Coin + "/H";
        foreach (char letter in value)
        {
            _txtValue.text += letter;
            yield return null;
        }
    }

    private void UpdateArtRelic()
    {
        _art.sprite = ResourceManager.Instance.GetRelicSpriteByID(_relicData.ID);
    }

    private void Start()
    {
        _btnClose.onClick.AddListener(OnClickClose);
    }

    private void OnDestroy()
    {
        _btnClose.onClick.RemoveListener(OnClickClose);
    }

    private void OnClickClose()
    {
        PopupManager.Instance.HidePopup<PopupGalleryRelic>();
    }
}
