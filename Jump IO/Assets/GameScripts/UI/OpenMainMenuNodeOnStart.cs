using GameScripts.UIManagement;
using UnityEngine;

namespace GameScripts.UI
{
    public class OpenMainMenuNodeOnStart : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.ShowViewNode(UINodeId.MainMenu);
        }
    }
}