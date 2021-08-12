using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        public enum Type { StartPath, PathrollingPath };
        public Type pathType;

        public Transform[] GetWaypoints()
        {
            return waypoints;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.25f);

                if ((i == (waypoints.Length - 1)) && (pathType == Type.PathrollingPath))
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                    return;
                }
                else if ((i != (waypoints.Length - 1)))
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }
}
