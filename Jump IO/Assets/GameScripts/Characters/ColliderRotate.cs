using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Characters
{
    public class ColliderRotate : MonoBehaviour
    {
        [SerializeField] private List<Transform> noRotate;

        public void Rotate(float angle)
        {
            foreach (var t in noRotate)
            {
                t.rotation = new Quaternion(0f, -angle, 0f, 0f);
            }
        }
    }
}