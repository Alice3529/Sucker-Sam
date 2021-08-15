using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace _Scripts.Player
{
    public class SearchWalls : MonoBehaviour
    {
        [SerializeField] Vector3 overlapWallsTB = new Vector3(0.28f, 2.43f, 0f);
        [SerializeField] Vector3 offsetTB = new Vector3(-0.65f, 0f, 0f);
        [SerializeField] LayerMask mask = new LayerMask();
        [SerializeField] Vector3 overlapWallsRL = new Vector3(0.28f, 2.43f, 0f);
        [SerializeField] Vector3 offsetRL = new Vector3(-0.65f, 0f, 0f);
        [SerializeField] int[] directions = new int[4] { 0, 0, 0, 0 }; //l,r,u,b
        PlayerMovememt.PlayerDirectionEnum currentDirection;
        Collider2D[] collidersTB= new Collider2D[0];
        Collider2D[] collidersRL= new Collider2D[0];
        List<Collider2D> colliders = new List<Collider2D>();


        public int[] GetDirections()
        {
            return directions;
        }
        void Update()
        {
            directions = new int[4] { 0, 0, 0, 0 };

            currentDirection = GetComponent<PlayerMovememt>().GetCurrentDirection();
            if ((currentDirection == PlayerMovememt.PlayerDirectionEnum.Right) ||
                (currentDirection == PlayerMovememt.PlayerDirectionEnum.Left))
            {
                SearchTopAndBottomColliders();
                SearchRightAndLeftColliders();

            }
            else if ((currentDirection == PlayerMovememt.PlayerDirectionEnum.Up) ||
                (currentDirection == PlayerMovememt.PlayerDirectionEnum.Down))
            {
                SearchRightAndLeftColliders();
                SearchTopAndBottomColliders();
            }
            collidersTB = new Collider2D[0];
            collidersRL = new Collider2D[0];
            colliders = new List<Collider2D>();
        }

        void SearchRightAndLeftColliders()
        {
            collidersRL = Physics2D.OverlapBoxAll(transform.position, overlapWallsRL, 90, mask);//collider
            foreach (Collider2D col in collidersRL)
            {
                if (colliders.Contains(col)) { return; }
                Vector3 colWalls = col.gameObject.transform.position + new Vector3(col.offset.x, col.offset.y, 0);
                if (colWalls.x > transform.position.x)
                {
                    directions[1] = 1;
                    colliders.Add(col);
                }
                else if (colWalls.x < transform.position.x)
                {
                    directions[0] = 1;
                    colliders.Add(col);

                }
            }
        }

        void SearchTopAndBottomColliders()
        {
            collidersTB = Physics2D.OverlapBoxAll(transform.position, overlapWallsTB, transform.rotation.z, mask);
            foreach (Collider2D col in collidersTB)
            {
                if (colliders.Contains(col)) { return; }
                Vector3 colWalls = col.gameObject.transform.position + new Vector3(col.offset.x, col.offset.y, 0);
                if (colWalls.y > transform.position.y)
                {
                    directions[2] = 1;
                    colliders.Add(col);

                }
                else if (colWalls.y < transform.position.y)
                {
                    directions[3] = 1;
                    colliders.Add(col);

                }

            }
        }

        void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero + offsetTB, overlapWallsTB);
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero + offsetRL, overlapWallsRL);
        }
    }
}
