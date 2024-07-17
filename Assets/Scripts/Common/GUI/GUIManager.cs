using System.Collections.Generic;
using UnityEngine;

public class GUIManager : PersistentMonoSingleton<GUIManager>
{
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
        HideAllScreen();
        HideAllPopup();

        ScreenBase screen = screenManager.GetScreen<T>();
        //screen.OnSetup();
        screen.OnInit(paras);
        screen.Show();       

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

    public T ShowPopup<T>(params object[] paras) where T : PopupBase
    {
        PopupBase popup = popupManager.GetPopup<T>();

        popup.OnSetup();
        popup.OnInit(paras);
        popup.Show();

        return popup as T;
    }

    public void HidePopup<T>(float delay = 0) where T : PopupBase
    {
        if (popupManager.CheckPopupShowed<T>())
        {
            popupManager.GetPopup<T>().HideByDelay(delay);
        }
        else
        {
            Debug.Log("Error Hide Screen");
        }
    }

    public void HideAllPopup()
    {
        List<PopupBase> popupShowed = popupManager.GetPopupsShowed();

        foreach (PopupBase popup in popupShowed)
        {
            popup.Hide();
        }
    }

    private void HideAllScreen()
    {
        List<ScreenBase> screenShowed = screenManager.GetSceensShowed();

        foreach (ScreenBase screen in screenShowed)
        {
            screen.Hide();
        }
    }
}
