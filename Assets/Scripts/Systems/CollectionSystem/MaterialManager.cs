using System;
using System.Collections;
using UnityEngine;

namespace CollectionSystem
{
    public class MaterialManager : MonoBehaviour
    {
        [SerializeField]
        private Material _Material;
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
            _posEnd = gadgetHeader.Material.transform.position;
        }

        public void VFX_ShowMaterial(int amountMaterial)
        {
            int amount = Mathf.FloorToInt(amountMaterial / 3f);
            this.amount = amount;
            Action callback_1 = () =>
            {
                MainPlayer.Instance.AddMaterial(4);
            };
            StartCoroutine(IE_ShowMaterial(amount, callback_1));

            if(amountMaterial > amount * 3)
            {
                this.amount += 1;
                Action callback_2 = () =>
                {
                    MainPlayer.Instance.AddMaterial(amountMaterial - amount * 3);
                };
                StartCoroutine(IE_ShowMaterial(1, callback_2));
            }
        }

        public float GetTime()
        {
            return (this.amount * this.timeDelay) + this.timeMove;
        }


        IEnumerator IE_ShowMaterial(int amount, Action callback)
        {
            yield return new WaitForEndOfFrame();
            ShowMaterial(amount, callback);
        }

        private void ShowMaterial(int amount, Action callback)
        {
            for (int i = 0; i < amount; i++)
            {
                Material Material = SpawnMaterial(_TF_Start.position + OffsetCircle(radius));
                Material.TweenMoving(_posEnd, timeMove, i * timeDelay, callback);
                Material.TweenScale(Vector3.zero, Vector3.one, i * timeDelay);
            }
        }

        private Material SpawnMaterial(Vector3 position)
        {
            //position += OffsetCircle(3f);
            Material Material = Instantiate<Material>(_Material, transform);
            Material.transform.position = position;
            return Material;
        }

        private Vector3 OffsetCircle(float radius)
        {
            return UnityEngine.Random.insideUnitCircle * radius;
        }
    }
}
