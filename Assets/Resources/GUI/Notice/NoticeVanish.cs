using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeVanish : MonoBehaviour
{
    [SerializeField]
    private GameObject _content;
    [SerializeField]
    private TMP_Text _txtName;
    [SerializeField]
    private TMP_Text _txtDescription;
    [SerializeField]
    private Image _art;

    private RectTransform m_RectTransform;
    private StackVanishData _vanishData;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        OnSetup();
    }

    public void OnInit(StackVanishData data)
    {
        _vanishData = data;

        UpdateName();
        UpdateDescirption();
        UpdateArt();
    }

    public void OnShow()
    {
        LeanTween.moveLocalY(_content, 800f, 0.3f)
            .setFrom(0f)
            .setEaseOutBack();

        LeanTween.scaleY(_content, 1f, 0.3f)
            .setFrom(0f)
            .setEaseOutBack();

        LeanTween.scaleX(_content, 1f, 0.3f)
            .setFrom(0.8f)
            .setEaseOutBack();
    }    

    public void OnHide()
    {
        LeanTween.scaleY(_content, 0f, 0.2f)
            .setFrom(1f)
            .setEaseInExpo()
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

    private void OnSetup()
    {
        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 1920 / 1080f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            //m_OffsetY = 100f;
        }
    }
}
