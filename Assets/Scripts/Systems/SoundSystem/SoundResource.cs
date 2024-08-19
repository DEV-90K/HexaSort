using UnityEngine;

namespace Audio_System
{
    public class SoundResource : MonoSingleton<SoundResource>
    {
        [SerializeField]
        public SoundData Confetti;
        [SerializeField]
        public SoundData Completed;
        [SerializeField]
        public SoundData Failed;
        [SerializeField]
        public SoundData SwordThrust;
    }
}
