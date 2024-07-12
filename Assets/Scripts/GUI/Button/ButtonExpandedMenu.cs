using UnityEngine;
using UnityEngine.UI;

public interface IExpandedMenu
{
    public void CloseMenu();
}

[RequireComponent(typeof(Button))]
public class ButtonExpandedMenu : MonoBehaviour, IExpandedMenu
{    
    private RectTransform[] _ItemExpands;
    private ButtonExpandedItem[] _Items;    
    private Vector3 _Spacing = new Vector3(0f, -148f, 0f);
    private Button _btn;
    private bool _isExpanded = false;
    private RectTransform _rect;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _rect = GetComponent<RectTransform>();

        _ItemExpands = new RectTransform[transform.childCount];
        _Items = new ButtonExpandedItem[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _ItemExpands[i] = transform.GetChild(i).GetComponent<RectTransform>();
            _ItemExpands[i].position = transform.position;
            _ItemExpands[i].gameObject.SetActive(false);

            _Items[i] = transform.GetChild(i).GetComponent<ButtonExpandedItem>();
            _Items[i].OnInit(this);
        }
    }

    private void Start()
    {
        _btn.onClick.AddListener(ToggleMenu);
        //OnResertMenu();
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(ToggleMenu);
    }

    //private void OnResertMenu()
    //{
    //    foreach(RectTransform tf in _ItemExpands)
    //    {
    //        tf.position = transform.position;
    //        tf.gameObject.SetActive(false);
    //    }
    //}

    private void ToggleMenu()
    {
        Debug.Log("On Toggle Menu");
        _isExpanded = !_isExpanded;

        if (_isExpanded) 
            ShowItems(); 
        else 
            HideItems();
    }

    private void ShowItems()
    {
        for (int i = 0; i < _ItemExpands.Length; i++)
        {
            _ItemExpands[i].gameObject.SetActive(true);
            Vector3 space = _rect.position + _Spacing * (i + 1);
            LeanTween.move(_ItemExpands[i], space, 0.02f)
                .setEaseOutBack();

            LeanTween.alpha(_ItemExpands[i], 1f, 0.02f)
                .setFrom(0f)
                .setEaseOutBack();
        }
    }

    private void HideItems()
    {
        foreach (RectTransform tf in _ItemExpands)
        {
            LeanTween.move(tf, transform.position, 0.02f)
                .setEaseInBack();
            LeanTween.alpha(tf, 0f, 0.02f)
                .setFrom(1f)
                .setEaseInBack()
                .setOnComplete(() =>
                {
                    tf.gameObject.SetActive(false);
                });
        }
    }
    public void CloseMenu()
    {
        _isExpanded = false;
        HideItems();
    }
}
