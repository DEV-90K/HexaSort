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
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, playerHexagon);

        if (hit.collider == null)
        {
            Debug.Log("Not detected any hexagon");
            return;
        }

        Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
        _able.OnBoostHammer(hexagon);

        StartCoroutine(IE_OnBoostHammerCompleted());
    }

    private IEnumerator IE_OnBoostHammerCompleted()
    {
        yield return new WaitForSeconds(2f);
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);
        Hide();
    }    
}
