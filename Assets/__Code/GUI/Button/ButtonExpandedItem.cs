using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonExpandedItem : MonoBehaviour
{
    protected IExpandedMenu Owner { get; private set; }
    public void OnInit(IExpandedMenu Owner)
    {
        this.Owner = Owner;
    }

    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        Owner.CloseMenu();
    }
}
