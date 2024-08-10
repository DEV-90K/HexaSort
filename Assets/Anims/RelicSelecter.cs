using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelecter : MonoBehaviour
{
    [SerializeField]
    private Animator _Animator;
    [SerializeField]
    private Image _Art;

    private Sprite _Sprite;

    public void Anim_OnInit(Sprite sprite)
    {
        _Sprite = sprite;
       // this.Anim_OnShow();
        _Animator.SetTrigger("TrgSelected");
    }

    private void OnEnable()
    {
        _Animator.SetTrigger("TrgSelected");
    }

    public void Anim_OnShow()
    {
        _Art.sprite = _Sprite;
    }

    public void Anim_OnShowCompleted()
    {
       // _Animator.SetBool("IsSelected", false);
    }
}
