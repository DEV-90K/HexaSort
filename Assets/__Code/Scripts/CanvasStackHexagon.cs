using TMPro;
using UnityEngine;
using Mul21_Lib;

public class CanvasStackHexagon : MonoBehaviour
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
        //Vector3.forward: (0, 0, 1)
        //WorldUp: thông thường sẻ là (0, 1, 0) => camera.transform.rotation * Vector3.up để chuyển đổi worldUp = worldspace của camera
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        Vector3 angle = transform.localEulerAngles;
        angle.x = 90;
        transform.localEulerAngles = angle;

        Debug.DrawRay(transform.position, transform.position + camera.transform.rotation * Vector3.forward, Color.magenta);
        
        Debug.DrawRay(camera.transform.position, camera.transform.rotation * Vector3.up, Color.green);
        Debug.DrawRay(camera.transform.position, camera.transform.rotation * Vector3.forward, Color.blue);
        Debug.DrawRay(camera.transform.position, camera.transform.rotation * Vector3.right, Color.red);
        Debug.DrawRay(transform.position, transform.rotation * Vector3.forward, Color.blue);
    }
}
