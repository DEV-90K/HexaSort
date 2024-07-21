using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    [SerializeField]
    private Transform screenRoot;

    private ScreenBase[] _screenPrefabs;

    private Dictionary<System.Type, ScreenBase> screens = new Dictionary<System.Type, ScreenBase>();
    private Dictionary<System.Type, ScreenBase> cacheScreens = new Dictionary<System.Type, ScreenBase>();
    public Dictionary<string, T_HexaInBoardData> hexasSelected;

    public int colorNumber;
    public int hexInEachHexaNumber;

    #region Screens

    private void Awake()
    {
        instance = this;
        ClearScreens();
        hexasSelected = new Dictionary<string, T_HexaInBoardData>();
        this.hexasSelected = T_Data.Instance._hexasSelected;
        this.colorNumber = T_Data.Instance.colorNumber;
        this.hexInEachHexaNumber = T_Data.Instance.hexInEachHexaNumber;
        //DontDestroyOnLoad(this);
    }

    public void CacheData()
    {
        //T_Data.Instance.hexasSelected = hexasSelected;
        T_Data.Instance.colorNumber = colorNumber;
        T_Data.Instance.hexInEachHexaNumber = hexInEachHexaNumber;
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
        cacheScreens[typeof(T)] = screen;
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

    public List<ScreenBase> GetSceensShowed()
    {
        List<ScreenBase> list = new List<ScreenBase>();
        foreach (KeyValuePair<System.Type, ScreenBase> item in cacheScreens)
        {
            if (item.Value != null && item.Value.gameObject.activeSelf)
            {
                list.Add(item.Value);
            }
        }

        return list;
    }


    public bool CheckScreenShowed<T>() where T : ScreenBase
    {
        if(CheckScreen<T>() && CheckScreenShowedFromCache<T>())
        {
            return true;
        }

        return false;
    }

    private bool CheckScreenShowedFromCache<T>() where T : ScreenBase
    {
        System.Type type = typeof(T);
        return cacheScreens[type].gameObject.activeSelf;
    }

    //Check Screen Cache or not
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
