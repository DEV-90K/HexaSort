using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHexagon : MonoBehaviour
{
    [SerializeField]
    private new Renderer renderer;

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }


}
