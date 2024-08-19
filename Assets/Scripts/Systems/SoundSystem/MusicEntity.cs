using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{
    public class MusicEntity : MonoBehaviour
    {
        [SerializeField]
        private MusicData[] audioClips;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("1");
                MusicManager.Instance.Play(audioClips[0]);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("2");
                MusicManager.Instance.Play(audioClips[1]);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("3");
                MusicManager.Instance.Play(audioClips[2]);
            }
        }

        
    }
}
