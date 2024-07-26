using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [SerializeField]
    private Transform _DialougeRoot;
    [SerializeField]
    private DialougeBox _DialougeBox;

    public void ShowDialougeBox(DialogueData data)
    {
        DialougeBox box = Instantiate(_DialougeBox, _DialougeRoot);
        box.OnInit(data);
    }

    public void HideDialougeBox()
    {
        _DialougeRoot.transform.DestroyChildrenImmediate();
    }
}
