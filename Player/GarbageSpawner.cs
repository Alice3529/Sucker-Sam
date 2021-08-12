using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GarbageSpawner : MonoBehaviour
{
    public List<GameObject> spawnTemplate = new List<GameObject>();
    public GameObject spawnPoint;
    public GameObject SpawnedGarbageParent;
    public float spawnRadius = 10f;
    public float wallDetectionRadius = 1f;
    public bool spawnAtStartOnly = true;
    public bool spawnDuringUpdate = false;
    public int spawnRandomEachMinSecond = 10;
    public int spawnRandomEachMaxSecond = 15;
    
    public int spawnObjects = 10;
    public bool randomX = true;
    public bool randomY = true;
    public bool randomZ = true;
    public bool randomZrotation = true;

    //public bool setZfromGarbageParent = false;
    
    private float lastCheckTime = 0f;
    private int nextRandomSpawnTime = 0;

    private void Start()
    {
        nextRandomSpawnTime = spawnRandomEachMinSecond;
        
        if (!spawnAtStartOnly) return;
        SpawnRandomObjects();
    }

    private void SpawnRandomObjects()
    {
        for (int i = 0; i < spawnObjects; i++)
            spawnRandomInRadius(spawnRadius);
    }

    private void Update()
    {
        if (!spawnDuringUpdate) return;

        //check if it is time to spawn
        lastCheckTime += Time.deltaTime;
        if (lastCheckTime < nextRandomSpawnTime) return;

        lastCheckTime = 0;
        nextRandomSpawnTime = Random.Range(spawnRandomEachMinSecond, spawnRandomEachMaxSecond);
        SpawnRandomObjects();
    }


    public GameObject spawnRandomInRadius(float radius)
    {
        if (!spawnTemplate.Any()) return null;

        var whatToSpawnIndex = Random.Range(0, spawnTemplate.Count);
        var whatToSpawn = spawnTemplate[whatToSpawnIndex];

        var centralPoint = Vector3.zero;
        for (var i = 0; i < 500; i++)
            if (SpawnFreePositionInRadius(whatToSpawn, radius, out centralPoint)) break;

        var targetRotation = whatToSpawn.transform.rotation;
        if (randomZrotation)
            targetRotation.z = Random.value;
            
        var result = Instantiate(whatToSpawn, centralPoint, targetRotation);
        result.transform.parent = (SpawnedGarbageParent) ? SpawnedGarbageParent.transform : spawnPoint.transform;

        return result;
    }

    /*
    private void SetZPosition(GameObject result)
    {
        if (setZfromGarbageParent && SpawnedGarbageParent)
        {
            var newPosition = result.transform.position;
            newPosition.z = SpawnedGarbageParent.transform.position.z;
            result.transform.position = newPosition;
        }
    }*/

    private bool SpawnFreePositionInRadius(GameObject whatToSpawn, float radius, out Vector3 centralPoint)
    {
        centralPoint = spawnPoint.transform.position;
        
        if (randomX)
            centralPoint.x += (float) Math.Sin(Random.value * 2 - 1) * radius;

        if (randomY)
            centralPoint.y += (float) Math.Sin(Random.value * 2 - 1) * radius;

        if (randomZ)
            centralPoint.z += (float) Math.Sin(Random.value * 2 - 1) * radius;

        
        //not spaw if at that place is something
        return IsSpotFree(centralPoint, whatToSpawn);
    }

    private bool IsSpotFree(Vector3 centralPoint, GameObject whatToSpawn)
    {
        List<GameObject> wallsCollision = new List<GameObject>();
       var allHits = Physics2D.OverlapCircleAll(centralPoint, wallDetectionRadius);
       foreach (var rhit in allHits)
        {
            if (rhit.gameObject.tag == "wall")
            {
                wallsCollision.Add(rhit.gameObject);
            }
        }
       var hit = wallsCollision?.Count > 0;
       return !hit;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(spawnPoint.transform.position, spawnPoint.transform.forward.normalized, spawnRadius);
        Gizmos.DrawWireSphere(spawnPoint.transform.position, spawnRadius);
    }
    #endif
}
