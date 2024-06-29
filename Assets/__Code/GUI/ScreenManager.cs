using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private Transform screenRoot;

    private ScreenBase[] _screenPrefabs;

    private ScreenBase _currentScreen;

    public ScreenBase CurrentScreen
    {
        get { return _currentScreen; }
        private set { _currentScreen = value; }
    }

    private Dictionary<System.Type, ScreenBase> screens = new Dictionary<System.Type, ScreenBase>();
    private Dictionary<System.Type, ScreenBase> cacheScreens = new Dictionary<System.Type, ScreenBase>();

    #region Screens

    private void Awake()
    {
        ClearScreens();
    }
    private void ClearScreens()
    {

        for (var i = screenRoot.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(screenRoot.GetChild(i).gameObject);
        }

        cacheScreens.Clear();
    }

    public T CreateScreen<T>() where T : ScreenBase
    {
        ScreenBase screen = Instantiate(GetPrefab<T>(), screenRoot);

        if (screen.canReused)
            cacheScreens.Add(typeof(T), screen);

        return screen as T;
    }

    public T GetScreen<T>() where T : ScreenBase
    {
        if (!CheckScreen<T>())
        {
            return CreateScreen<T>();
        }

        return cacheScreens[typeof(T)] as T;
    }

    public List<T> GetScreensShowed<T>() where T : ScreenBase
    {
        List<T> list = new List<T>();
        foreach (KeyValuePair<System.Type, ScreenBase> item in cacheScreens)
        {
            if (item.Value != null && item.Value.gameObject.activeInHierarchy)
            {
                list.Add(item.Value as T);
            }
        }

        return list;
    }

    public bool CheckScreenShowed<T>() where T : ScreenBase
    {
        if(CheckScreen<T>() && cacheScreens[typeof(T)].gameObject.activeInHierarchy)
        {
            return true;
        }

        return false;
    }

    private bool CheckScreen<T>() where T : ScreenBase
    {
        System.Type type = typeof(T);
        return cacheScreens.ContainsKey(type) && cacheScreens[type] != null;
    }    

    private T LoadPrefabs<T>() where T : ScreenBase
    {
        if(!screens.ContainsKey(typeof(T)))
        {
            if(_screenPrefabs == null)
            {
                _screenPrefabs = Resources.LoadAll<ScreenBase>("GUI/Screen");
            }

            for(int i = 0; i < _screenPrefabs.Length; i++)
            {
                if (_screenPrefabs[i] is T)
                {
                    screens.Add(typeof(T), _screenPrefabs[i]);
                    break;
                }
            }
        }

        return screens[typeof(T)] as T;
    }

    private T GetPrefab<T>() where T : ScreenBase
    {
        if (screens.ContainsKey(typeof(T)))
        {
            return screens[typeof(T)] as T;
        }

        return LoadPrefabs<T>() as T;
    }
    #endregion
}
