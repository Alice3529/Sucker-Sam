using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;


public interface IGhostEndPoint 
{
    public GameObject FindEndPointInChaseMode(Transform cell, float maxDistance1, Transform player);
}
