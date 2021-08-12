using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;


[RequireComponent(typeof(Pathfinding))]
public class Blinky : MonoBehaviour, IGhostEndPoint
{
   public GameObject FindEndPointInChaseMode(Transform cell, float maxDistance1, Transform player)
   {
        if (Vector3.Distance(cell.position, player.position) < maxDistance1)
        {
            maxDistance1 = Vector3.Distance(cell.position, player.position);
            GetComponent<Pathfinding>().SetMaxDistance(maxDistance1);
            return cell.gameObject;
        }
        return null;
   }

    public GameObject DoConverting(Transform cell, Transform player)
    {
        return null;
    } 
}
