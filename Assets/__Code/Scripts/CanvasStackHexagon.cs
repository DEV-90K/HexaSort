using TMPro;
using UnityEngine;

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
}
