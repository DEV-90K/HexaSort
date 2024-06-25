
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    #region Popup
    [SerializeField]
    private Transform popupRoot;
    
    private PopupBase[] _popupPrefabs;

    Dictionary<System.Type, PopupBase> popups = new Dictionary<System.Type, PopupBase>();
    Dictionary<System.Type, PopupBase> cachePopups = new Dictionary<System.Type, PopupBase>();

    private void Awake()
    {
        ClearPopups();
    }
    private void ClearPopups()
    {

        for (var i = popupRoot.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(popupRoot.GetChild(i));
        }

        cachePopups.Clear();
    }


    public T CreatePopup<T>() where T : PopupBase
    {
        PopupBase popup = Instantiate(GetPrefab<T>(), popupRoot);

        if(popup.canReused) 
            cachePopups.Add(typeof(T), popup);
        else
            cachePopups.Add(typeof(T), null);

        return popup as T;
    }

    public T GetPopup<T>() where T : PopupBase
    {
        if(!CheckPopup<T>())
        {
            return CreatePopup<T>();
        }

        return cachePopups[typeof(T)] as T;
    }

    public List<T> GetPopupsShowed<T>() where T : PopupBase
    {
        List<T> list = new List<T>();
        foreach (KeyValuePair<System.Type, PopupBase> item in cachePopups)
        {
            if(item.Value != null && item.Value.gameObject.activeInHierarchy)
            {
                list.Add(item.Value as T);
            }
        }

        return list;
    }

    public bool CheckPopupShowed<T>() where T : PopupBase
    {
        if (CheckPopup<T>() && cachePopups[typeof(T)].gameObject.activeInHierarchy)
        {
            return true;
        }

        return false;
    }
    private bool CheckPopup<T>() where T : PopupBase
    {
        System.Type type = typeof(T);
        return cachePopups.ContainsKey(type) && cachePopups[type] != null;
    }

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
