using System;
using System.Collections;
using UnityEngine;

namespace CollectionSystem
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField]
        private Coin _Coin;
        [SerializeField]
        private RectTransform _TF_Start;
        private Vector3 _posEnd;

        public float radius = 4f;
        public float timeMove = 1f;
        public float timeDelay = 0.04f;
        private int amount = 10;

        private void Start()
        {
            GadgetHeader gadgetHeader = GUIManager.Instance.GadgetHeader;
            _posEnd = gadgetHeader.Coin.transform.position;
        }

        public void VFX_ShowCoin(int amountCoin)
        {
            int amount = Mathf.FloorToInt(amountCoin/4f);
            this.amount = amount;
            Action callback_1 = () =>
            {
                MainPlayer.Instance.AddCoin(4);
            };
            StartCoroutine(IE_ShowCoin(amount, callback_1));

            if(amountCoin > amount * 4)
            {
                this.amount += 1;

                Action callback_2 = () =>
                {
                    MainPlayer.Instance.AddCoin(amountCoin - amount * 4);
                };
                StartCoroutine(IE_ShowCoin(1, callback_2));
            }
        }

        public float GetTime()
        {
            return (this.amount * this.timeDelay) + this.timeMove;
        }

        private IEnumerator IE_ShowCoin(int amount, Action callback)
        {
            yield return new WaitForEndOfFrame();
            ShowCoin(amount, callback);
        }

        private void ShowCoin(int amount, Action callback)
        {
            for (int i = 0; i < amount; i++)
            {
                Coin coin = SpawnCoin(_TF_Start.position + OffsetCircle(1f));
                coin.TweenMoving(_posEnd, timeMove, i * timeDelay, callback);
                coin.TweenScale(Vector3.zero, Vector3.one, i * timeDelay);
            }
        }

        private Coin SpawnCoin(Vector3 position)
        {
            //position += OffsetCircle(3f);
            Coin coin = Instantiate<Coin>(_Coin, transform);
            coin.transform.position = position;
            return coin;
        }

        private Vector3 OffsetCircle(float radius)
        {
            return UnityEngine.Random.insideUnitCircle * radius;
        }
    }
}
