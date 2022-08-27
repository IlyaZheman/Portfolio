using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class GameItemSlot : MonoBehaviour
    {
        [SerializeField] private int radius = 50;

        public string ItemId { get; private set; }

        private void Awake()
        {
            ItemId = GetComponent<Image>().sprite.name;

            gameObject.AddComponent<CircleCollider2D>();
            GetComponent<CircleCollider2D>().radius = radius;
        }
    }
}