using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDots : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Dots;

    //the total time of the animation
    public float repeatTime = 1;

    //the time for a dot to bounce up and come back down
    public float bounceTime = 0.25f;

    //how far does each dot move
    public float bounceHeight = 8f;

    void Start()
    {
        if (repeatTime < Dots.Length * bounceTime)
        {
            repeatTime = Dots.Length * bounceTime;
        }
        InvokeRepeating("Animate", 0, repeatTime);
    }

    void Animate()
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            LTSeq seq = LeanTween.sequence();
            seq.append(i * bounceTime / 2);
            seq.append(LeanTween.moveLocalY(Dots[i], bounceHeight, bounceTime / 2));
            seq.append(LeanTween.moveLocalY(Dots[i], 0, bounceTime / 2));            
        }
    }
}
