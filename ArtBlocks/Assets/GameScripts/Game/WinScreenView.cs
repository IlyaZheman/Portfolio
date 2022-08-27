using UnityEngine;

namespace GameScripts.Game
{
    public class WinScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject winCanvas;
        [SerializeField] private GameObject levelNameCanvas;
        [SerializeField] private GameObject gamePanel;

        public void WinScreenEnable()
        {
            winCanvas.SetActive(true);
            levelNameCanvas.SetActive(false);
            gamePanel.SetActive(false);
        }

        public void WinScreenDisable()
        {
            winCanvas.SetActive(false);
            levelNameCanvas.SetActive(true);
            gamePanel.SetActive(true);
        }
    }
}