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
    [SerializeField]
    private ParticleSystem[] particles;

    private ParticleSystem particle;

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

    public void OnShow(Vector3 wPos, int number)
    {
        isFollowing = true;
        gameObject.SetActive(true);
        transform.position = wPos;
        UpdateTxtNumber(number);       
    }

    public IEnumerator OnShowAnim(VanishType type = VanishType.NONE)
    {   
        

        if (type != VanishType.NONE)
        {
            img.sprite = ResourceManager.Instance.GetStackVanishSprite(type);
            animator.SetBool("IsShow", true);
            hasAnim = true;

            yield return WaitUntilAnimCompleted();
        }
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

    public IEnumerator PlayParticle(VanishType type)
    {
        txtNumber.gameObject.SetActive(false);
        switch (type)
        {
            case VanishType.RANDOM:
                particle = particles[0];
                break;
            case VanishType.AROUND:
                particle = particles[1];
                break;
            case VanishType.CONTAIN_COLOR:
                particle = particles[2];
                break;
        }

        particle.Play();
        yield return new WaitForSeconds(2f);
        particle.Stop();
        //yield return new WaitUntil(() => particle.isPlaying == false);
        txtNumber.gameObject.SetActive(true);
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
}
