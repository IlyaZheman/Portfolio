using GameScripts.Providers.Interface;
using UnityEngine;

namespace GameScripts.Providers.Module
{
    public class InputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] private float deadZone = 0.2f;
        private FloatingJoystick _joystick;
        
        private void Awake()
        {
            _joystick = FindObjectOfType<FloatingJoystick>();
        }

        public bool MoveLeft()
        {
            return _joystick.Horizontal < -deadZone;
        }

        public bool MoveRight()
        {
            return _joystick.Horizontal > deadZone;
        }
    }
}