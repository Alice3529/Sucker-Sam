using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;


[RequireComponent(typeof(Pathfinding))]
public class Clyde : MonoBehaviour, IGhostEndPoint
{
    [SerializeField] float distance;
    int startCurrent=0;
    bool canChase = true;

    public GameObject FindEndPointInChaseMode(Transform cell, float maxDistance1, Transform player)
    {
        if ((Vector3.Distance(player.position, this.gameObject.transform.position) > distance) )
        {
            return ScatterMovementNew(GetComponent<EnemyAI1>().GetPatrollingPath());
        }
        else
        {
            if (Vector3.Distance(cell.position, player.position) < maxDistance1)
            {
                maxDistance1 = Vector3.Distance(cell.position, player.position);
                GetComponent<Pathfinding>().SetMaxDistance(maxDistance1);
                return cell.gameObject;
            }
        }
        return null;
    }

    public GameObject DoConverting(Transform cell, Transform player)
    {
        return null;
    }

    GameObject ScatterMovementNew(List<Transform> startWaypoints)
    {
        if (transform.position == new Vector3(startWaypoints[startCurrent].position.x, startWaypoints[startCurrent].position.y, 0))
        {
            startCurrent = (startCurrent + 1) % startWaypoints.Count;
        }
    
        return startWaypoints[startCurrent].gameObject;
    }

}
