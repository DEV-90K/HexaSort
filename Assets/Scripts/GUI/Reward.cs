using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUIChestReward
{
    public class Reward : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem Glow_01_VFX;
        [SerializeField]
        private Transform IconPos;
        [SerializeField]
        private ParticleSystem StunStarExplosion;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _amount;

        private ChestRewardData _data;

        public void OnInit(ChestRewardData chestRewarData, Vector3 worldPos, Vector3 targetPos)
        {
            _data = chestRewarData;
            _icon.sprite = ResourceManager.Instance.GetRewardSpriteByType(_data.Type);
            _amount.text = "+ " + _data.Amount.ToString() + " " + EnumUtils.ParseString(_data.Type);


            transform.position = worldPos;
            TweenMoving(targetPos);
        }

        private void TweenMoving(Vector3 targetPos)
        {
            Glow_01_VFX.Stop();
            StunStarExplosion.Stop();

            LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.5f)
                .setFrom(Vector3.one * 0.8f)
                .setEaseInBack();

            LeanTween.move(gameObject, targetPos, 0.5f)
                .setEaseInBack()
                .setOnComplete(() =>
                {
                    OnMoveCompleted();
                });
        }

        private void OnMoveCompleted()
        {

            Glow_01_VFX.Play();
            StunStarExplosion.Play();
            //VFX_Glow_01();
            //VFX_StunStarExplosion();
        }

        private void VFX_Glow_01()
        {
            Instantiate(Glow_01_VFX, IconPos.position, Quaternion.identity, transform);
        }

        private void VFX_StunStarExplosion()
        {
            Instantiate(StunStarExplosion, IconPos.position, Quaternion.identity, transform);
        }
    }
}
