using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoSingleton<NoticeManager>
{
    [SerializeField]
    private Transform _NoticeRoot;
    [SerializeField]
    private NoticeVanish _NotiveVanish;
    [SerializeField]
    private Canvas _Canvas;
    [SerializeField]
    private Camera _Camera;

    private NoticeVanish _currentNotiveVanish = null;

    public void ShowNoticeVanish(StackVanishData data)
    {
        if(_currentNotiveVanish != null)
        {
            _currentNotiveVanish.StopAllCoroutines();
            _currentNotiveVanish.OnHide();
        }

        _currentNotiveVanish = SpawnNoticeVanish(data);
        StartCoroutine(IE_ShowNoticeVanish());
    }

    private IEnumerator IE_ShowNoticeVanish()
    {
        _currentNotiveVanish.OnShow();
        yield return new WaitForSeconds(0.2f); //Time Tween Show
        yield return new WaitForSeconds(1.6f); //Time Show
        _currentNotiveVanish.OnHide();
        yield return new WaitForSeconds(0.2f); //Time Hide
    }

    private NoticeVanish SpawnNoticeVanish(StackVanishData data)
    {
        NoticeVanish noti = Instantiate<NoticeVanish>(_NotiveVanish, _NoticeRoot);
        noti.OnInit(data);

        return noti;
    }
}
