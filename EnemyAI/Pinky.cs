using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class Pinky : MonoBehaviour, IGhostEndPoint
{
    enum LookAt { left, right, up, down }
    LookAt look;

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
        return ChangeCell(cell);
    }


    private GameObject ChangeCell(Transform cell)
    {
        Vector2 coords = cell.GetComponent<Tile>().GetCoords();
        if (look == LookAt.right)
        {
            coords += new Vector2(4,0);
        }
        else if (look == LookAt.left)
        {
            coords -= new Vector2(4,0);
        }
        else if (look == LookAt.up)
        {
            coords += new Vector2(-4, 4);
        }
        else if (look == LookAt.down)
        {
            coords -= new Vector2(0, 4);
        }
        Dictionary<Vector2, GameObject> dic = GetComponent<Pathfinding>().GetDic();
        if (dic.ContainsKey(coords))
        {
            return dic[coords];
        }
        return null;
    }

    private void GetPlayerRotation(Transform player)
    {
        float angle = player.eulerAngles.z;
        if (angle < 0)
        {
            angle = ((int)angle / 360 + 1)*360 + angle;
        }
        else
        {
            angle =angle-360*((int)angle / 360);
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
}
