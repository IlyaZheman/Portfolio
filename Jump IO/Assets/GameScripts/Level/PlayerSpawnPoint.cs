using UnityEngine;

namespace GameScripts.Level
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.25f);
            Gizmos.color = Color.white;
        }
    }
}