using UnityEngine;

namespace GameScripts.Level
{
    public class BotSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.25f);
            Gizmos.color = Color.white;
        }
    }
}