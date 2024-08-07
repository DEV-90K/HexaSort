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

    public IEnumerator IE_ShowNoticeVanish(StackVanishData data)
    {
        NoticeVanish noti = Instantiate<NoticeVanish>(_NotiveVanish, _NoticeRoot);
        noti.OnInit(data);
        noti.OnShow();
        yield return new WaitForSeconds(0.2f); //Time Tween Show
        yield return new WaitForSeconds(1.6f); //Time Show
        noti.OnHide();
        yield return new WaitForSeconds(0.2f); //Time Hide
    }
}
