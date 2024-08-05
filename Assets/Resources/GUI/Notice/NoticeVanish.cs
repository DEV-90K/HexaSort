using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeVanish : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txtName;
    [SerializeField]
    private TMP_Text _txtDescription;
    [SerializeField]
    private Image _art;

    private StackVanishData _vanishData;

    public void OnInit(StackVanishData data)
    {
        _vanishData = data;

        UpdateName();
        UpdateDescirption();
        UpdateArt();
        transform.localScale = Vector3.zero;
    }

    public void OnShow()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.2f)
            .setFrom(Vector3.one * 0.85f)
            .setEaseInExpo();
    }

    public void OnHide()
    {
        LeanTween.scale(gameObject, Vector3.one * 0.85f, 0.2f)
            .setFrom(Vector3.one)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private void UpdateName()
    {
        _txtName.text = _vanishData.Name;
    }

    private void UpdateDescirption()
    {
        _txtDescription.text = _vanishData.Description;
    }

    private void UpdateArt()
    {
        _art.sprite = ResourceManager.Instance.GetStackVanishSprite(_vanishData.Type);
    }
}
