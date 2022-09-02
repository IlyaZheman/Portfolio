using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class BotSliderColorChanger : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void ChangeColor(Color color)
        {
            image.color = color;
        }
    }
}