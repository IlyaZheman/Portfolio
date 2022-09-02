using UniRx;
using UnityEngine;

namespace GameScripts.Level
{
    public class Platform : MonoBehaviour
    {
        public BoxCollider2D boxCollider2d;
        public IReactiveProperty<int> Health;

        private void Awake()
        {
            Health = new ReactiveProperty<int>(1000);
        }

        public virtual void TakeHealth()
        {
            if (Health.Value <= 0) return;
            Health.Value--;
            if (Health.Value == 0)
            {
                boxCollider2d.enabled = false;
            }
        }
    }
}