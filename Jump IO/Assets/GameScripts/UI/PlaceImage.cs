using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class PlaceImage : MonoBehaviour
    {
        public Image image;
        public PlaceImagesCatalog placeImagesCatalog;

        public void Display(int place)
        {
            image.sprite = placeImagesCatalog.GetSpriteByPlace(place);
        }
    }
}