using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suck : MonoBehaviour
{
    [SerializeField] Transform dots;
    [SerializeField] Transform trunk;
    [SerializeField] float range;
    [SerializeField] float dotSpeed = 3f;

    void FixedUpdate()
    {
        for (int i=0; i<dots.transform.childCount; i++)
        {
            float distance = Vector3.Distance(trunk.position, dots.transform.GetChild(i).position);
            Vector3 normalizedDistance = (trunk.position - dots.transform.GetChild(i).position).normalized;
            if (distance <= range)
            {
                dots.GetChild(i).GetComponent<Rigidbody2D>().AddForce(normalizedDistance * dotSpeed * Time.deltaTime); 
            }
        }
    }

}
