using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;
using System;


public class Pathfinding : MonoBehaviour
{
    GameObject startPoint;
    GameObject player;
    GameObject endPoint;
    [SerializeField] Transform grid;
    float maxDistance = 1000f;
    float maxDistance1 = 1000f;
    Dictionary<Vector2, GameObject> dic = new Dictionary<Vector2, GameObject>();
    Vector2[] directions = {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
    Queue<Transform> closeCells = new Queue<Transform>();
    bool isRunning=true;
    [SerializeField] List<Transform> backPath = new List<Transform>();
    public event Action<List<Transform>> newPath;
    EnemyAI1 enemy;
    List<Transform> lockCellsTransform=new List<Transform>();

    void Awake()
    {
        GetComponent<EnemyAI1>().lockFull += IsLocked;
    }

    #region get and set
    public Transform GetGrid()
    {
        return grid;
    }

    public Dictionary<Vector2, GameObject> GetDic()
    {
        return dic;
    }

    public void IsLocked(List<Transform> cellsToLock)
    {
        lockCellsTransform = cellsToLock;
    }
    #endregion

    void Start()
    {
        player = FindObjectOfType<PlayerMovememt>().gameObject;
        dic = FindObjectOfType<DotsAutoPlays>().GetDic();
        enemy = GetComponent<EnemyAI1>();
    }


    public void SearchPath(Transform scatterModePoint)
    {
        ThrowOffPreviousPath();
        FindStartAndEnd(scatterModePoint);
        ColorStartAndEnd();
        EnemyPathfinding();
    }


    private void ThrowOffPreviousPath()
    {
        backPath.Clear();
        closeCells = new Queue<Transform>();
      
    }

    private void FindStartAndEnd(Transform scatterModePoint)
    {
        foreach (Transform cell in grid)
        {
            cell.GetComponent<Tile>().SetIsChecked(false);
            cell.GetComponent<Tile>().SetParent(null);

            if (Vector3.Distance(cell.position, gameObject.transform.position) < maxDistance)
            {
                maxDistance = Vector3.Distance(cell.position, gameObject.transform.position);
                startPoint = cell.gameObject;
            }

            if (enemy.IsInScatterMode())
            {
                endPoint = scatterModePoint.gameObject;
            }
            else if (enemy.IsInChasingMode() || (enemy.IsInFrightenedMode()))
            {
                GameObject point = GetComponent<IGhostEndPoint>().FindEndPointInChaseMode(cell, maxDistance1, player.transform);
                if (point != null)
                {
                    endPoint = point;
                }
            }
        }
        if (endPoint != null)
        {
            if ((gameObject.tag == "inky" && enemy.IsInChasingMode()) ||
                (gameObject.tag == "pinky" && enemy.IsInChasingMode()))
            {
                if (GetComponent<IGhostEndPoint>().DoConverting(endPoint.transform, player.transform) != null)
                {
                    endPoint = GetComponent<IGhostEndPoint>().DoConverting(endPoint.transform, player.transform);

                }
            }
            startPoint.GetComponent<Tile>().SetIsChecked(true);
            maxDistance = 1000f;
            maxDistance1 = 1000f;
        }
    }

    private void ColorStartAndEnd()
    {
        closeCells.Enqueue(startPoint.transform);
        Color(startPoint);
        Color(endPoint);
    }

    void Color(GameObject pointToColor)
    {
        if (!pointToColor.GetComponent<Tile>().IsPatrollingPoint())
        {
            pointToColor.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        }
    }

    private void EnemyPathfinding()
    {
        if (closeCells.Count > 0 )
        {
             var searchCenter = closeCells.Dequeue();
             StopIfTheEndFound(searchCenter);
             ExploreNeighbours(searchCenter);

        }
    }

    private void ExploreNeighbours(Transform currentCell)
    {
        foreach (Vector2 direction in directions)
        {
            Vector2 nearCell = new Vector2(currentCell.position.x, currentCell.position.y) + direction;
            if (dic.ContainsKey(nearCell))
            {
                Transform neighbour = dic[nearCell].transform;
                Tile tile = neighbour.gameObject.GetComponent<Tile>();
                if (!tile.GetIsChecked() && !lockCellsTransform.Contains(tile.transform))
                {
                    print(tile.gameObject.name);
                    tile.SetParent(currentCell.gameObject);
                    if (!tile.IsPatrollingPoint())
                    {
                        neighbour.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.3f, 0.3f);
                    }
                    closeCells.Enqueue(neighbour);
                    tile.SetIsChecked(true);
                }
            }
        }
        EnemyPathfinding(); 
    }

    

    private void StopIfTheEndFound(Transform currentCell)
    {
        if (currentCell == endPoint.transform)
        {
            FindPathBack(endPoint.transform);
        }
    }

    private void FindPathBack(Transform backPoint)
    {
        while (backPoint.GetComponent<Tile>().GetParent() != null)
        {
            if (!backPoint.GetComponent<Tile>().IsPatrollingPoint())
            {
                backPoint.GetComponent<SpriteRenderer>().color = new Color(0.3745199f, 0.6509434f, 0.2855553f);
            }
            backPath.Add(backPoint);
            if (backPoint.GetComponent<Tile>().GetParent() == null) {
                return; 
            }
            backPoint = backPoint.GetComponent<Tile>().GetParent();
        }
        backPath.Reverse();
        if (!enemy.IsInFrightenedMode() || enemy.inCage)
        {
            newPath(backPath);
        }
        else
        {
            FrightenedMode(startPoint.transform);
        }
    }

    private void FrightenedMode(Transform currentCell)
    {
        float distance = 0;
        Transform neighbourToAdd=null;
        foreach (Vector2 direction in directions)
        {
            Vector2 nearCell = new Vector2(currentCell.position.x, currentCell.position.y) + direction;
            if (dic.ContainsKey(nearCell) && (nearCell != new Vector2(backPath[0].transform.position.x, backPath[0].transform.position.y)))
            {
                Transform neighbour = dic[nearCell].transform;
                if (Vector3.Distance(neighbour.transform.position, endPoint.transform.position) > distance)
                {
                    distance = Vector3.Distance(neighbour.transform.position, endPoint.transform.position);
                    neighbourToAdd = neighbour;
                }
            }
        }
        backPath.Clear();
        backPath.Add(neighbourToAdd);
        newPath(backPath);

    }

    public List<Transform> GetBackPath()
    {
        return backPath;
    }

    public void SetMaxDistance(float distance)
    {
        maxDistance1=distance;
    }

}
