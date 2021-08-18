using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class Inky : MonoBehaviour, IGhostEndPoint
{
    enum LookAt { left, right, up, down }
    LookAt look;
    Transform blinky;
    List<Vector2> corners=new List<Vector2>();
    Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };




    void Start()
    {
        corners = FindObjectOfType<DotsAutoPlays>().GetCorners();
        blinky = GameObject.FindGameObjectWithTag("blinky").transform;
        if (blinky == null)
        {
            Debug.LogError("There is no blinky in scene. Code can not calculate inky's path");
        }
    }

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
        GetPlayerRotation(player);
        GameObject changedCell = ChangeCell(cell);
        if (changedCell != null)
        {
          return CellReflection(changedCell);
        }
        return null;
    }


    private GameObject ChangeCell(Transform cell)
    {
        Vector2 coords = cell.GetComponent<Tile>().GetCoords();
        if (look == LookAt.right)
        {
            coords += new Vector2(2, 0);
        }
        else if (look == LookAt.left)
        {
            coords -= new Vector2(2, 0);
        }
        else if (look == LookAt.up)
        {
            coords += new Vector2(0, 2);
        }
        else if (look == LookAt.down)
        {
            coords -= new Vector2(0, 2);
        }
        Dictionary<Vector2, GameObject> dic = GetComponent<Pathfinding>().GetDic();

        if (dic.ContainsKey(coords))
        {
            return dic[coords];
        }
        return cell.gameObject;
      
    }

    private void GetPlayerRotation(Transform player)
    {
        float angle = player.eulerAngles.z;
        if (angle < 0)
        {
            angle = ((int)angle / 360 + 1) * 360 + angle;
        }
        else
        {
            angle = angle - 360 * ((int)angle / 360);
        }

        if (angle >= 45 && angle <= 135)
        {
            look = LookAt.down;
        }
        else if (angle > 135 && angle < 225)
        {
            look = LookAt.right;
        }
        else if (angle >= 225 && angle <= 315)
        {
            look = LookAt.up;
        }
        else if (angle > 315 && angle < 405)
        {
            look = LookAt.left;
        }
    }

    private GameObject CellReflection(GameObject cell)
    {
        Vector2 cellCoords = cell.GetComponent<Tile>().GetCoords();
        Vector2 blinkyCoords = blinky.GetComponent<EnemyAI1>().GetEnemyCoords();
        Vector2 cellDistance = cellCoords-blinkyCoords;
        Vector2 newPoint = cellCoords+cellDistance;
        Dictionary<Vector2, GameObject> dic = GetComponent<Pathfinding>().GetDic();

        if (dic.ContainsKey(newPoint))
        {
            return dic[newPoint];
        }
        return cell;
       
    }

    public Vector2 FindClosestPoint(Vector2 currentCell)
    {
        Dictionary<Vector2, GameObject> dic = GetComponent<Pathfinding>().GetDic();
        foreach (Vector2 direction in directions)
        {
            Vector2 nearCell = currentCell + direction;
            if (dic.ContainsKey(nearCell))
            {
                return nearCell;
            }
        }
        return currentCell;
    }
}
