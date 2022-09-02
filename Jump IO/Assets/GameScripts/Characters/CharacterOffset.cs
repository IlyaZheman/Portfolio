using UnityEngine;

namespace GameScripts.Characters
{
    public class CharacterOffset : MonoBehaviour
    {
        [SerializeField] private Transform characterTransform;

        private int _offset;

        private void Update()
        {
            var position = characterTransform.position;
            characterTransform.position = new Vector3(position.x, position.y, (position.y * Mathf.Tan(60)) - _offset);
        }

        public void ChangeOffset(int numberOfCharacter)
        {
            _offset = numberOfCharacter;
        }
    }
}