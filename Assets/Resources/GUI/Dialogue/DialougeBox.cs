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

    private DialogueData _data;
    public Queue<string> _sequences = new Queue<string>();

    public void OnInit(DialogueData data)
    {
        _data = data;

        _sequences.Clear();
        foreach (string sequence in _data.Sentences)
        {
            _sequences.Enqueue(sequence);
        }

        UpdateName();
        UpdateConversation();
    }

    private void UpdateName()
    {
        _Name.text = _data.Name;
    }

    private void UpdateConversation()
    {
        string sequence = _sequences.Dequeue();
        _Conversation.text = sequence;

        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        if (_sequences.Count == 0)
        {
            _Navigation.text = "Skip >>";
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
            DestroyImmediate(gameObject);
        }
        else
        {
            UpdateConversation();
        }
    }

    private void Start()
    {
        _BtnNavigation.onClick.AddListener(OnClickNavigation);
    }

    private void OnDestroy()
    {
        _BtnNavigation.onClick.RemoveListener(OnClickNavigation);
    }
}
