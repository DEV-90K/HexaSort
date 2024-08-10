using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    private Canvas _Canvas;

    private void Awake()
    {
        _Canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if(GameManager.Instance.IsState(GameState.LEVEL_PLAYING))
        {
            _Canvas.sortingOrder = 3;
        }
        else
        {
            _Canvas.sortingOrder = -1;
        }
    }
}
