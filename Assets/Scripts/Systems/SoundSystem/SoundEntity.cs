using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{
    public class SoundEntity : MonoBehaviour
    {
        [SerializeField]
        private SoundData soundData;
        TimerUtils.CountdownTimer clockTime;
        private bool CanShoot = true;
        private void Awake()
        {
            clockTime = new TimerUtils.CountdownTimer(0.2f);
            clockTime.OnTimerStart += () => CanShoot = false;
            clockTime.OnTimerStop += () => CanShoot = true;            
        }

        private void Update()
        {            
            if (Input.GetMouseButton(0) && CanShoot == true)
            {
                clockTime.Start();
                SpawnSFX();
            }

            clockTime.Tick(Time.deltaTime);
        }

        private void SpawnSFX()
        {
            SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(soundData);
        }
    }
}
