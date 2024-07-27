using System;
using UnityEngine;

public class Hexagon : PoolMember
{
    public static Action OnVanish;

    [SerializeField]
    private new Renderer renderer;
    [SerializeField]
    private new Collider collider;
    [SerializeField]
    private GameObject model;
    public StackHexagon HexagonStack { get; private set; }
    public Color Color
    {
        get => renderer.material.color;

        set
        {
            renderer.material.color = value;

            if(_data == null)
            {

            }
        }
    }

    private Color normalColor;
    private Color trickColor;

    private HexagonData _data;

    public void OnSetUp()
    {
        this.transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        EnableCollider();
    }

    public void OnInit(HexagonData data)
    {
        _data = data;

        if (ColorUtility.TryParseHtmlString(data.HexColor, out Color color))
        {
            Color = color;
            normalColor = color;
            ColorUtility.TryParseHtmlString("#525252", out trickColor);
        }
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

    public void TweenShowTrick()
    {
        LeanTween.cancel(model);
        LeanTween.color(model, trickColor, 1f).setLoopPingPong();
    }

    public void TweenHideTrick()
    {
        LeanTween.cancel(model);
        Color = normalColor;
    }

    public void MoveToGridHexagon(Vector3 localPos, float delayTime)
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, localPos, GameConstants.HexagonConstants.TIME_ANIM)
            .setEaseInOutSine()
            .setDelay(delayTime);

        Vector3 direction = (localPos - transform.localPosition).With(y: 0).normalized;
        Vector3 v = transform.rotation * direction;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, v);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, GameConstants.HexagonConstants.TIME_ANIM)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                transform.localEulerAngles = Vector3.zero;
            })
            .setDelay(delayTime);
    }

    public void TweenVanish(float offsetDelayTime)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, GameConstants.HexagonConstants.TIME_ANIM)
            .setEaseInBack()
            .setDelay(offsetDelayTime)
            .setOnComplete(() =>
                {
                    Collect();
                }
             );
    }

    public void OnResert()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        EnableCollider();
        HexagonStack = null;
    }

    public void CollectImmediate()
    {
        OnResert();
        PoolManager.Despawn(this);
    }

    public void Collect()
    {
        OnResert();
        PoolManager.Despawn(this);
        OnVanish?.Invoke();
    }

    public bool CheckColor(Color color)
    {
        return ColorUtils.ColorEquals(color, Color);
    }

    #region Hexagon Data
    internal HexagonData GetCurrentHexagonPlayingData()
    {      
        return _data;
    }
    #endregion Hexagon Data
}
