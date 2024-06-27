using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class Hexagon : MonoBehaviour
{
    [SerializeField]
    private new Renderer renderer;
    [SerializeField]
    private new Collider collider;
    public StackHexagon HexagonStack { get; private set; }
    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }

    public void Configure(StackHexagon hexStack)
    {
        HexagonStack = hexStack;
    }

    public void SetParent(Transform newHexStack)
    {
        transform.SetParent(newHexStack);
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void MoveToGridHexagon(Vector3 localPos)
    {
        LeanTween.cancel(gameObject);
        float delay = transform.GetSiblingIndex() * 0.01f + 0.01f; //0.01f of GridHexagon

        LeanTween.moveLocal(gameObject, localPos, 0.2f)
            .setEaseInOutSine()
            .setDelay(delay);

        Vector3 direction = (localPos - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, 0.2f)
            .setEaseInOutSine()
            .setDelay(delay);
    }

    public void TweenVanish(float offsetDelayTime)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f)
            .setEaseInBack()
            .setDelay(offsetDelayTime)
            .setOnComplete(() => 
                DestroyImmediate(gameObject)
             );
    }
}
