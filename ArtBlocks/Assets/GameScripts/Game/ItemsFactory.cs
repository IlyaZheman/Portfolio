using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class ItemsFactory : MonoBehaviour
    {
        [SerializeField] private GameItem itemPrefab;
        [SerializeField] private GameObject parentOfItemSlot;
        [SerializeField] private Transform dragHandlerContainer;

        public List<GameItem> allItems;

        private List<Sprite> _sprites;

        public void CreateItems()
        {
            FindAllSpritesAtCurrentLevel();

            foreach (var sprite in _sprites)
            {
                var item = Instantiate(itemPrefab, transform);
                var itemImage = item.GetComponent<Image>();
                itemImage.sprite = sprite;
                itemImage.SetNativeSize();
                item.GetComponent<DraggableItem>().DragHandlerContainer = dragHandlerContainer;
                item.GetComponent<GameItem>().Init();
                item.gameObject.SetActive(false);
                allItems.Add(item);
            }
        }

        public void DestroyItems()
        {
            _sprites.Clear();
            foreach (var gameItem in allItems)
            {
                Destroy(gameItem.gameObject);
            }

            allItems.Clear();
        }

        private void FindAllSpritesAtCurrentLevel()
        {
            _sprites = new List<Sprite>();
            var images = parentOfItemSlot.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                _sprites.Add(image.sprite);
            }
        }
    }
}