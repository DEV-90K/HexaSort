using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStackHexagon : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text txtNumber;

    [SerializeField]
    private Image img;

    [SerializeField]
    private Transform stack;
    [SerializeField]
    private Animator animator;

    private StackHexagon _stackHexagon;

    private bool hasAnim = false;
    private bool isFollowing = false;

    private void OnEnable()
    {
        isFollowing = true;
    }

    private void OnDisable()
    {
        isFollowing = false;
    }

    private void Awake()
    {        
        canvas.worldCamera = Camera.main;
    }

    public void OnHide()
    {
        isFollowing = false;
        gameObject.SetActive(false);
    }

    public void OnShow()
    {
        isFollowing = true;
        gameObject.SetActive(true);
        OnUpdate();      
    }

    public void OnUpdate()
    {
        transform.position = _stackHexagon.GetTopPosition();
        int number = _stackHexagon.GetNumberSimilarTopColor();
        UpdateTxtNumber(number);
    }

    public IEnumerator WaitUntilAnimCompleted()
    {   
        yield return new WaitUntil(() => hasAnim == false);
    }

    public void AnimShowed()
    {
        animator.SetBool("IsShow", false);
        hasAnim = false;
    }

    public void UpdateTxtNumber(int number)
    {
        txtNumber.text = number.ToString();
    }

    private void Update()
    {
        if (isFollowing)
        {
            Camera cam = Camera.main;
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            txtNumber.transform.LookAt(stack.position, Quaternion.Euler(90, 0, 0) * (transform.rotation * Vector3.up));           
        }
    }

    public void OnInit(StackHexagon stackHexagon)
    {
        _stackHexagon = stackHexagon;
    }
}
