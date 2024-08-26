
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
    #region Popup
    [SerializeField]
    private Transform popupRoot;
    
    private PopupBase[] _popupPrefabs;

    Dictionary<System.Type, PopupBase> popups = new Dictionary<System.Type, PopupBase>();
    //Dictionary<System.Type, PopupBase> cachePopups = new Dictionary<System.Type, PopupBase>();

<<<<<<< HEAD
    Dictionary<System.Type, PopupBase> cacheEnable = new Dictionary<System.Type, PopupBase>();
    Dictionary<System.Type, PopupBase> cacheDisable = new Dictionary<System.Type, PopupBase>();

=======
>>>>>>> parent of 99c86cc (Update)
    protected override void Awake()
    {
        base.Awake();
        ClearPopups();
    }
    private void ClearPopups()
    {

        for (var i = popupRoot.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(popupRoot.GetChild(i).gameObject);
        }

<<<<<<< HEAD
        //cachePopups.Clear();

        {
            cacheEnable.Clear();
            cacheDisable.Clear();
        }
=======
        cachePopups.Clear();
>>>>>>> parent of 99c86cc (Update)
    }


    public T CreatePopup<T>() where T : PopupBase
    {
        PopupBase popup = Instantiate(GetPrefab<T>(), popupRoot);
        //cachePopups[typeof(T)] = popup;
        return popup as T;
    }

    public T ShowPopup<T>() where T : PopupBase
    {
<<<<<<< HEAD
        T popup = GetPopup<T>();
        popup.OnSetup();
        popup.Show();
=======
        if(!CheckPopup<T>())
        {
            return CreatePopup<T>();
        }
>>>>>>> parent of 99c86cc (Update)

        CacheShowed<T>(popup);
        return popup;
    }

<<<<<<< HEAD
    private void CacheShowed<T>(T popup) where T : PopupBase
=======

    public List<PopupBase> GetPopupsShowed()
>>>>>>> parent of 99c86cc (Update)
    {
        cacheDisable[typeof(T)] = null;
        cacheEnable[typeof(T)] = popup;
    }

    public void HidePopup<T>(float delay = 0) where T : PopupBase
    {
        T popup = GetPopupEnable<T>();
        popup.HideByDelay(delay);
        CacheHided<T>(popup);
    }

    public void HideAllPopup()
    {
        List<System.Type> keys = new List<System.Type>(cacheEnable.Keys);
        foreach (System.Type key in keys)
        {
            PopupBase value = cacheEnable[key];
            if (value != null)
            {
                value.HideByDelay(0);
                cacheEnable[key] = null;
                cacheDisable[key] = value;
            }
        }
    }

    public void CacheHided<T>(T popup) where T : PopupBase
    {
        cacheEnable[typeof(T)] = null;
        cacheDisable[typeof(T)] = popup;        
    }

    private T GetPopup<T>() where T : PopupBase
    {
        //if (!CheckPopup<T>())
        //{
        //    return CreatePopup<T>();
        //}

        //return cachePopups[typeof(T)] as T;

        T popup = GetPopupEnable<T>();
        if (popup != null) 
            return popup;

        popup = GetPopupDisable<T>();
        if (popup != null)
        {
            return popup;
        }

        popup = CreatePopup<T>();
        return popup;
    }

    private bool CheckPopupEnable<T>() where T : PopupBase
    {
        System.Type type = typeof(T);
        return cacheEnable.ContainsKey(type) && cacheEnable[type] != null;
    }

    public T GetPopupEnable<T>() where T : PopupBase
    {
        if(!CheckPopupEnable<T>())
        {
            Debug.Log("Popup not enable");
            return null;
        }

        System.Type type = typeof(T);
        return cacheEnable[type] as T;

    }

    private bool CheckPopupDisable<T>() where T : PopupBase
    {
        System.Type type = typeof(T);
        return cacheDisable.ContainsKey(type) && cacheDisable[type] != null;
    }

    private T GetPopupDisable<T>() where T : PopupBase
    {
        if (!CheckPopupDisable<T>())
        {
            Debug.Log("Popup not disable");
            return null;
        }

        System.Type type = typeof(T);
        return cacheDisable[type] as T;

    }

    //public List<T> GetPopupsShowed<T>() where T : PopupBase
    //{
    //    //List<PopupBase> list = new List<PopupBase>();
    //    //foreach (KeyValuePair<System.Type, PopupBase> item in cachePopups)
    //    //{
    //    //    if (item.Value != null && item.Value.gameObject.activeSelf)
    //    //    {
    //    //        list.Add(item.Value);
    //    //    }
    //    //}

    //    //return list;

    //    List<T> list = new List<T>();
    //    foreach (KeyValuePair<System.Type, PopupBase> item in cacheEnable)
    //    {
    //        if(item.Value != null)
    //        {
    //            list.Add(item.Value);
    //        }
    //    }

    //    return list;
    //}

    public bool CheckAnyPopupShowed()
    {
        foreach (KeyValuePair<System.Type, PopupBase> item in cacheEnable)
        {
            if(item.Value != null)
            {
                return true;
            }
        }

        return false;
    }

    //public bool CheckPopupShowed<T>() where T : PopupBase
    //{
    //    if (CheckPopup<T>() && CheckPopupShowedFromCache<T>())
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private bool CheckPopupShowedFromCache<T>() where T : PopupBase
    //{
    //    System.Type type = typeof(T);
    //    return cachePopups[type].gameObject.activeSelf;
    //}

    //private bool CheckPopupShowedFromCache<T>() where T : PopupBase
    //{
    //    return CheckPopupEnable<T>();
    //}

    //private bool CheckPopup<T>() where T : PopupBase
    //{
    //    System.Type type = typeof(T);
    //    return cachePopups.ContainsKey(type) && cachePopups[type] != null;
    //}

    private T LoadPrefabs<T>() where T : PopupBase
    {
        if (!popups.ContainsKey(typeof(T)))
        {
            if (_popupPrefabs == null)
            {
                _popupPrefabs = Resources.LoadAll<PopupBase>("GUI/Popup");
            }

            for (int i = 0; i < _popupPrefabs.Length; i++)
            {
                if (_popupPrefabs[i] is T)
                {
                    popups.Add(typeof(T), _popupPrefabs[i]);
                    break;
                }
            }
        }

        return popups[typeof(T)] as T;
    }

    private T GetPrefab<T>() where T : PopupBase
    {
        if(popups.ContainsKey(typeof(T)))
        {
            return popups[typeof(T)] as T;
        }

        return LoadPrefabs<T>() as T;
    }
    #endregion

    //Press Back Key on android device
    //// Multi Popup display at same time, Them can from to any at same time
    //#region Back track Popup 
    //private List<PopupBase> popupBackables = new List<PopupBase>();
    //private Dictionary<PopupBase, UnityAction> popupBackActions = new Dictionary<PopupBase, UnityAction>();

    //public PopupBase PopupBackableTop
    //{
    //    get
    //    {
    //        PopupBase popup = null;
    //        if (popupBackables.Count > 0)
    //        {
    //            popup = popupBackables[popupBackables.Count - 1];
    //        }

    //        return popup;
    //    }
    //}


    ////private void LateUpdate()
    ////{
    ////    if (Input.GetKey(KeyCode.Escape) && PopupBackableTop != null)
    ////    {
    ////        popupBackActions[PopupBackableTop]?.Invoke();
    ////    }
    ////}

    //public void PushBackAction(PopupBase popup, UnityAction action)
    //{
    //    if (!popupBackActions.ContainsKey(popup))
    //    {
    //        popupBackActions.Add(popup, action);
    //    }
    //}

    //public void AddPopupBackable(PopupBase popup)
    //{
    //    if (!popupBackables.Contains(popup))
    //    {
    //        popupBackables.Add(popup);
    //    }
    //}

    //public void RemovePopupBackable(PopupBase popup)
    //{
    //    popupBackables.Remove(popup);
    //}

    ///// <summary>
    ///// CLear backey when comeback index UI canvas
    ///// </summary>
    //public void ClearBackKey()
    //{
    //    popupBackables.Clear();
    //}
    //#endregion Back track Popup
}
