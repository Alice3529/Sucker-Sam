using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsAutoPlays : MonoBehaviour
{
    [SerializeField] GameObject dotObj;
    [SerializeField] Vector2 leftbottomcorner;
    [SerializeField] Vector2 righttopcorner;
    [SerializeField] GameObject dotsParent;
    List<Vector2> corners = new List<Vector2>();
    Dictionary<Vector2, GameObject> dic = new Dictionary<Vector2, GameObject>();

    void Awake()
    {
        for (float i = leftbottomcorner.y; i <= righttopcorner.y; i++)
        {
            for (float m = leftbottomcorner.x; m <= righttopcorner.x; m++)
            {
                GameObject newObj = Instantiate(dotObj, new Vector3(m, i, 0), Quaternion.identity) as GameObject;
                newObj.name = $"{m},{i}";
                newObj.transform.parent = dotsParent.transform;
                newObj.GetComponent<Tile>().SetCoords(new Vector2(m, i));
                dic.Add(new Vector2(m, i), newObj);
              

            }
        }
        corners.Add(leftbottomcorner);
        corners.Add(righttopcorner);

    }
    public Dictionary<Vector2, GameObject> GetDic()
    {
        return dic;
    }
    public List<Vector2> GetCorners()
    {
        return corners;
    }
}

 