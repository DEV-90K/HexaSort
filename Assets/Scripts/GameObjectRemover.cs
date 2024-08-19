using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRemover : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);
    }
}
