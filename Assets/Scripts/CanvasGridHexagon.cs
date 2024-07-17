using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGridHexagon : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text txtNumber;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void UpdateTxtNumber(int number)
    {
        txtNumber.text = number.ToString();
    }

    private void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        Vector3 angle = transform.localEulerAngles;
        angle.x = 90;
        transform.localEulerAngles = angle;
    }
}
