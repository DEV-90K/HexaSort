using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeBox : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _Name;
    [SerializeField]
    private TMP_Text _Conversation;
    [SerializeField]
    private TMP_Text _Navigation;
    [SerializeField]
    private Button _BtnNavigation;
    [SerializeField]
    private Button _BtnSkip;
    [SerializeField]
    private Button _BtnBG;
    [SerializeField]
    private Animator _animator;

    private DialogueData _data;
    private Action _actionSkip;
    public Queue<string> _sequences = new Queue<string>();

    public void OnInit(DialogueData data, Action actionSkip = null)
    {
        _animator.SetBool("IsOpen", true);
        _actionSkip = actionSkip;
        _BtnSkip.gameObject.SetActive(true);
        _data = data;
        _sequences.Clear();
        foreach (string sequence in _data.Sentences)
        {
            _sequences.Enqueue(sequence);
        }

        UpdateName();
    }

    public void OnExit()
    {
        if(_data.Type == DialogueType.CHEST_REWARD)
        {
            GUIManager.Instance.ShowPopup<PopupChestReward>();
        }
        else if(_data.Type == DialogueType.RELIC_COLLECT)
        {
            GUIManager.Instance.HideScreen<ScreenLevel>();
            GUIManager.Instance.HidePopup<PopupLevelWoned>();
            GUIManager.Instance.ShowPopup<PopupGallery>(1);
        }
        
        Destroy(gameObject);
    }

    private void UpdateName()
    {
        _Name.text = _data.Name;
    }

    public void UpdateConversation()
    {
        string sequence = _sequences.Dequeue();
        StartCoroutine(IE_TypeSentence(sequence));
    }

    private IEnumerator IE_TypeSentence(string sentence)
    {
        _Conversation.text = "";
        _Navigation.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _Conversation.text += letter;
            yield return null;
        }

        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        if (_sequences.Count == 0)
        {
            _Navigation.text = "Let's go >>";
        }
        else
        {
            _Navigation.text = "Continue >>";
        }
    }

    private void OnClickNavigation()
    {
        if(_sequences.Count == 0)
        {
            _animator.SetBool("IsOpen", false);
        }
        else
        {
            UpdateConversation();
        }
    }

    private void OnClickSkip()
    {
        _BtnSkip.gameObject.SetActive(false);

        if(_actionSkip != null)
        {
            _actionSkip.Invoke();
        }
        else if(_data.Type == DialogueType.RELIC_COLLECT)
        {
            GUIManager.Instance.HideScreen<ScreenLevel>();
            GUIManager.Instance.HidePopup<PopupLevelWoned>();
            GUIManager.Instance.ShowPopup<PopupGallery>(1);
        }
        else if (_data.Type == DialogueType.CHEST_REWARD)
        {
            GUIManager.Instance.ShowPopup<PopupChestReward>();
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        _BtnNavigation.onClick.AddListener(OnClickNavigation);
        _BtnBG.onClick.AddListener(OnClickNavigation);
        _BtnSkip.onClick.AddListener(OnClickSkip);
    }

    private void OnDestroy()
    {
        _BtnNavigation.onClick.RemoveListener(OnClickNavigation);
        _BtnBG.onClick.RemoveListener(OnClickNavigation);
        _BtnSkip.onClick.RemoveListener(OnClickSkip);
    }
}
