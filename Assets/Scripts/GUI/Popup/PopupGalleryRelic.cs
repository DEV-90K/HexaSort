using TMPro;
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
    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _data = (GalleryRelicData)paras[0];
        _relicData = ResourceManager.Instance.GetRelicDataByID(_data.IDRelic);

        UpdateArtRelic();
        UpdateTxtName();
        UpdateTxtDescription();
        UpdateTxtValue();
    }

    private void UpdateTxtName()
    {
        _txtName.text = _relicData.Name;
    }

    private void UpdateTxtDescription()
    {
        _txtDescription.text = "+ Description: " +_relicData.Description;
    }

    private void UpdateTxtValue()
    {
        _txtValue.text = "+ Display value: " +_relicData.Coin + "Coin" + "/" + _relicData.Timer + "Minute";
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
        Hide();
    }
}
