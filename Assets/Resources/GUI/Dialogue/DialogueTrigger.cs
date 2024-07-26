using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData _DialogueData;
    private Button _btnTrigger;
    private bool _IsTrigger = false;

    private void Awake()
    {
        _btnTrigger = GetComponent<Button>();
    }

    private void Start()
    {
        _btnTrigger.onClick.AddListener(OnTriggerDialogue);
    }

    private void OnDestroy()
    {
        _btnTrigger.onClick.RemoveListener(OnTriggerDialogue);
    }

    private void OnTriggerDialogue()
    {
        TriggerDialouge();
    }

    public void TriggerDialouge()
    {
        _IsTrigger = !_IsTrigger;

        if(_IsTrigger)
            DialogueManager.Instance.ShowDialougeBox(_DialogueData);
        else
            DialogueManager.Instance.HideDialougeBox();
    }
}
