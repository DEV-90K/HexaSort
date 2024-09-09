using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public interface IBoostHammer
{
    public void EnterBoostHammer();
    public void OnBoostHammer(Hexagon hexagon);
    public void ExitBoostHammer();
}

public class PopupBoostHammer : PopupBase
{
    public static Action<bool> OnStackMoving;

    [SerializeField]
    private Button btnClose;

    [SerializeField]
    private LayerMask playerHexagon;

    private RectTransform m_RectTransform;
    private Coroutine _hamming = null;
    private IBoostHammer _able;
    private LevelPresenterData _presenterData;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _able = (IBoostHammer)paras[0];
        _presenterData = LevelManager.Instance.GetPresenterData();
    }

    public override void Show()
    {
        base.Show();
        OnStackMoving?.Invoke(false);
        _able.EnterBoostHammer();

        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    public override void Hide()
    {
        base.Hide();
        OnStackMoving?.Invoke(true);
        _able.ExitBoostHammer();
    }

    private void OnClickBtnClose()
    {
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);
        Hide();
    }

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        OnSetup();
        btnClose.onClick.AddListener(OnClickBtnClose);
    }

    private void Update()
    {
        Controlling();
    }

    private void Controlling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Input All Collider diffirent Hexagon Close
            ControlMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            //Input All Collider Hexagon Close
            ControlMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ControlMouseUp();
        }
    }

    private void ControlMouseDown()
    {
        //hammer.transform.position = GetWorldPosFromMouseClicked();
        //Debug.Log("Hammer world pos: " + hammer.transform.position.ToString());
    }

    private void ControlMouseDrag()
    {
        
    }

    private void ControlMouseUp()
    {
        if (_hamming != null) 
            return;

        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, playerHexagon);

        if (hit.collider == null)
        {
            Debug.Log("Not detected any hexagon");
            return;
        }

        Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
        _able.OnBoostHammer(hexagon);

        _hamming = StartCoroutine(IE_OnBoostHammerCompleted());
    }

    private IEnumerator IE_OnBoostHammerCompleted()
    {
        yield return new WaitForSeconds(2f);
        _hamming = null;
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);
        Hide();
    }

    public override void OnSetup()
    {
        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 1920 / 1080f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            //m_OffsetY = 100f;
        }
    }
}
