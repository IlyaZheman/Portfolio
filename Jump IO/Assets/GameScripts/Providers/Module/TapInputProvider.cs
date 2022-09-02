using GameScripts.Providers.Interface;
using GameScripts.UI;
using UnityEngine;

namespace GameScripts.Providers.Module
{
    public class TapInputProvider : MonoBehaviour, IInputProvider
    {
        public PressableElement leftButton;
        public PressableElement rightButton;

        public bool MoveLeft()
        {
            return leftButton.Pressed && !rightButton.Pressed;
        }

        public bool MoveRight()
        {
            return rightButton.Pressed && !leftButton.Pressed;
        }
    }
}