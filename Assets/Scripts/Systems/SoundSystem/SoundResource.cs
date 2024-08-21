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
        [SerializeField]
        public SoundData CoinCollect;
        [SerializeField]
        public SoundData MaterialCollect;
        [SerializeField]
        public SoundData AoE_Ice_Storm;
        [SerializeField]
        public SoundData AoE_Starts;

        [SerializeField]
        public SoundData ButtonClick;
        [SerializeField]
        public SoundData ButtonCollectCoin;
        [SerializeField]
        public SoundData ButtonBooster;
        [SerializeField]
        public SoundData ButtonBuy;

        [SerializeField]
        public SoundData SelectRelic;

        [SerializeField]
        public SoundData ShowPopup;
        [SerializeField]
        public SoundData HidePopup;
    }
}
