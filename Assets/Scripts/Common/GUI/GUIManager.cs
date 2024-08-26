using System.Collections.Generic;
using UnityEngine;

public class GUIManager : PersistentMonoSingleton<GUIManager>
{
    [SerializeField]
    public GadgetHeader GadgetHeader;
    [SerializeField]
    private PopupManager popupManager;
    [SerializeField]
    private ScreenManager screenManager;

    protected override void Awake()
    {
        base.Awake();
        popupManager = GetComponent<PopupManager>();
        screenManager = GetComponent<ScreenManager>();
    }

    public T ShowScreen<T>(params object[] paras) where T : ScreenBase
    {
        HideAllPopup();
        ScreenBase screen = screenManager.GetScreen<T>();
        screen.OnSetup();
        screen.OnInit(paras);
        screen.Show();
        return screen as T;
    }

    public T GetScreen<T>() where T : ScreenBase
    {
        ScreenBase screen = screenManager.GetScreen<T>();
        return screen as T;
    }

    public void HideScreen<T>(float delay = 0) where T : ScreenBase
    {
        if(screenManager.CheckScreenShowed<T>())
        {
            screenManager.GetScreen<T>().HideByDelay(delay);
        }
        else
        {
            Debug.Log("Error Hide Screen");
        }
    }

    //public T ShowPopup<T>(params object[] paras) where T : PopupBase
    //{
    //    PopupBase popup = popupManager.GetPopup<T>();

    //    popup.OnSetup();
    //    popup.Show();

    //    return popup as T;
    //}

    //public void HidePopup<T>(float delay = 0) where T : PopupBase
    //{
    //    if (popupManager.CheckPopupShowed<T>())
    //    {
    //        popupManager.GetPopup<T>().HideByDelay(delay);
    //    }
    //    else
    //    {
    //        Debug.Log("Error Hide Screen");
    //    }
    //}

    public void HideAllPopup()
    {
        popupManager.HideAllPopup();
    }
}
