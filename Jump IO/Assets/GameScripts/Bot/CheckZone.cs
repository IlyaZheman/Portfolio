using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Bot
{
    public class CheckZone : MonoBehaviour
    {
        public List<Transform> PlatformsInTrigger { get; private set; }

        private void Awake()
        {
            PlatformsInTrigger = new List<Transform>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            PlatformsInTrigger.Add(col.gameObject.GetComponent<Transform>());
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            PlatformsInTrigger.Remove(col.gameObject.GetComponent<Transform>());
        }
    }
}