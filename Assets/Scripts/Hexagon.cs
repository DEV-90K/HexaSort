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
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private HexagonAudio _Audio;
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

        //PlaySpawn();
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

    public void TweenMovingToGrid(Vector3 localPos, float delayTime)
    {        
        Vector3 centerPoint = Vector3.Lerp(localPos, transform.localPosition, 0.5f).Add(y: GameConstants.HexagonConstants.HEIGHT_BONUS);

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, centerPoint, GameConstants.HexagonConstants.TIME_ANIM/2)
            .setEaseInSine()
            .setOnComplete(() =>
            {
                LeanTween.moveLocal(gameObject, localPos, GameConstants.HexagonConstants.TIME_ANIM/2)
                .setEaseOutSine();
            })
            .setDelay(delayTime);

        Vector3 direction = (localPos - transform.localPosition).With(y: 0).normalized;
        Vector3 v = transform.rotation * direction;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, v);

        LeanTween.rotateAround(gameObject, rotationAxis, 90, GameConstants.HexagonConstants.TIME_ANIM / 2)
            .setOnStart(() =>
            {
                PlayMerge();
            })
            .setEaseInSine()
            .setOnComplete(() =>
            {
                LeanTween.rotateAround(gameObject, rotationAxis, 90, GameConstants.HexagonConstants.TIME_ANIM / 2)
                    .setEaseOutSine()
                    .setOnComplete(() =>
                    {                        
                        transform.localEulerAngles = Vector3.zero;
                    });
            })
            .setDelay(delayTime);
    }

    public void TweenVanish(float offsetDelayTime)
    {        
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, GameConstants.HexagonConstants.TIME_ANIM)
            .setEaseInBack()
            .setDelay(offsetDelayTime)
            .setOnStart(() =>
            {
                PlaySort();
            })
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

    #region Hexagon Audio
    private void PlayMerge()
    {
        _Audio.PlayMerge();
    }

    private void PlaySort()
    {
        _Audio.PlaySort();
    }

    private void PlaySpawn()
    {
        _Audio.PlaySpawn();
    }
    #endregion Hexagon Audio
}
