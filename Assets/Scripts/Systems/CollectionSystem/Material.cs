using UnityEngine;

namespace CollectionSystem
{
    public class Material : MonoBehaviour
    {
        public void TweenMoving(Vector3 target, float duration, float delay = 0, System.Action callback = null)
        {
            LeanTween.move(gameObject, target, duration)
                .setDelay(delay)
                .setEaseInBack()
                .setOnComplete(() =>
                {
                    callback?.Invoke();
                    Destroy(gameObject);
                });
        }

        public void TweenScale(Vector3 from, Vector3 to, float duration)
        {
            LeanTween.scale(gameObject, to, duration).setFrom(from);
        }
    }
}
