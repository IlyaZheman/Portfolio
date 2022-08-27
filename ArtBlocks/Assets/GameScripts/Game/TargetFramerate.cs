using UnityEngine;

namespace GameScripts.Game
{
    public class TargetFramerate : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}