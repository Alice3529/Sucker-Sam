using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;
using System;
using Enemy;

[RequireComponent(typeof(Pathfinding))]
public class EnemyAI1 : MonoBehaviour
{
    Transform[] startWaypoints;
    [SerializeField] float enemySpeed = 1f;
    int damageForPlayer = 1;
    int current = 0;
    int startCurrent = 0;
    public bool IsPatroling;//do not del
    public PlayerMovememt player;
    private float timeAfterStart = 1.5f;
    [SerializeField] float timeAfterStartForEnemy;
    [SerializeField] GameObject wall;
    [SerializeField] float timeToWait = 2f;
    Pathfinding pathfinding;
    [SerializeField] GameObject grid;
    [SerializeField] List<Transform> currentPath = new List<Transform>();
    [SerializeField] List<string> patrollingPathDots = new List<string>();
    List<Transform> pathDotsTransform = new List<Transform>();
    [SerializeField] List<string> waitPathDots = new List<string>();
    List<Transform> waitDotsTransform = new List<Transform>();
    [SerializeField] bool chasingMode = false;
    [SerializeField] bool scatterMode = true;
    [SerializeField] bool frightenedMode = false;
    int scatterPointNumber = 0;
    float currentTime = 0f;
    int tr = 0;
    [SerializeField] List<float> timeModes = new List<float>() { 20f, 27f, 47f, 54f, 59f };
    [SerializeField] float timerInFrightened = 5f;
    [SerializeField] GameObject light;
    [SerializeField] bool waitMode = true;
    GameObject patdot;
    bool canMove = true;
    public bool inCage=true;
    string cellPinkyStart="2,5";
    [SerializeField] Vector2 enemyCoords;
    List<string> lockCells = new List<string>() { "1,7", "2,7" };
    List<Transform> lockCellsTransform= new List<Transform>();
    public event Action<List<Transform>> lockFull;

    public List<Transform> GetLockCells()
    {
        return lockCellsTransform;
    }

    public List<Transform> GetPatrollingPath()
    {
        return pathDotsTransform;
    }

    public Vector2 GetEnemyCoords()
    {
        return enemyCoords;
    }

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        pathfinding.newPath += GetPath;

    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovememt>();
        light.SetActive(false);
        foreach (string dot in patrollingPathDots)
        {
            foreach (Transform child in grid.transform)
            {
                if (child.gameObject.name == dot)
                {
                     patdot = child.gameObject;
                     break;
                }
            }
            if (patdot.GetComponent<Tile>() != null)
            {
                patdot.GetComponent<Tile>().SetIsPatrollingPoint();
            }
            patdot.GetComponent<SpriteRenderer>().color = new Color(1f, 0.95f, 0f);
            pathDotsTransform.Add(patdot.transform);

        }
        foreach (string dot in waitPathDots)
        {
            GameObject patdot = GameObject.Find(dot);
            patdot.GetComponent<Tile>().SetIsPatrollingPoint();
            patdot.GetComponent<SpriteRenderer>().color = new Color(1f, 0.95f, 0f);
            waitDotsTransform.Add(patdot.transform);

        }
        StartCoroutine("Wait");

    }
   

    void GetPath(List<Transform> list)
        {
            startCurrent = 0;
            currentPath = list;
        }

        public bool IsInScatterMode()
        {
            return scatterMode;
        }

        public void SetScatterMode()
        {
            scatterMode = true;
        }

    public void SetFrightenedMode()
    {
        frightenedMode = true;
        chasingMode = false;
        if (!inCage)
        {
            timerInFrightened = player.GetComponent<PlayerCollisionDetector>().GetPowerupTime();
            light.SetActive(true);
            scatterMode = false;
            currentPath = null;
            currentPath = null;
            chasingMode = false;
            GetComponent<Pathfinding>().SearchPath(null);

        }
     }

        public bool IsInFrightenedMode()
        {
            return frightenedMode;
        }
        public bool IsInChasingMode()
        {
            return chasingMode;
        }

    void Update()
    {
        if (canMove)
        {
            if ((!frightenedMode && !waitMode) )
            {
                Timer();
            }
            else if (frightenedMode && !inCage )
            {
                TimerInFrightened();
            }

            if (chasingMode)
            {
                if (currentPath.Count != 0)
                {
                    scatterMode = false;
                    ChasingMovement(currentPath);
                }
                else
                {
                    GetComponent<Pathfinding>().SearchPath(null);
                }
            }
            else if ((currentPath.Count != 0) && scatterMode)
            {
                chasingMode = false;
                ScatterMovement(currentPath);
            }

            else if ((currentPath.Count != 0) && frightenedMode == true && inCage == false)
            {
                if (light.active == false)
                {
                    light.SetActive(true);
                }
                ChasingMovement(currentPath);
            }
           
        }
    }

        void ChasingMovement(List<Transform> startWaypoints)
        {
            if (transform.position != new Vector3(startWaypoints[0].position.x, startWaypoints[0].position.y, 0))
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, startWaypoints[0].position, enemySpeed * Time.deltaTime);
                pos = new Vector3(pos.x, pos.y, 0);
                transform.position = pos;
        } 
        else
            {
                enemyCoords = new Vector2(startWaypoints[0].position.x, startWaypoints[0].position.y);
                pathfinding.SetIsPathCalculated();
                GetComponent<Pathfinding>().SearchPath(null);

            }

        }

        void ScatterMovement(List<Transform> startWaypoints)
        {
            if (transform.position != new Vector3(startWaypoints[startCurrent].position.x, startWaypoints[startCurrent].position.y, 0))
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, startWaypoints[startCurrent].position, enemySpeed * Time.deltaTime);
                pos = new Vector3(pos.x, pos.y, 0);
                transform.position = pos;

            }
            else
            {
                pathfinding.SetIsPathCalculated();
                enemyCoords = new Vector2(startWaypoints[startCurrent].position.x, startWaypoints[startCurrent].position.y);
            if (startCurrent != (startWaypoints.Count - 1))
                {
                      startCurrent = startCurrent + 1;
                }
                else

                    if (!waitMode)
                    {
                       scatterPointNumber = (scatterPointNumber + 1) % pathDotsTransform.Count;
                       GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);
                    }
                    else 
                    {
                       scatterPointNumber = (scatterPointNumber + 1) % waitDotsTransform.Count;
                       GetComponent<Pathfinding>().SearchPath(waitDotsTransform[scatterPointNumber]);
                    }
                }
            }
        


        void Timer()
        {
            currentTime += Time.deltaTime;
            if (tr >= (timeModes.Count)) { return; }
            if (currentTime > timeModes[tr])
            {
                ChangeEnemyMode();
                tr += 1;
            }
        }

        void TimerInFrightened()
        {
            if (player.GetComponent<PlayerUpdateStatsScreen>().IsTimeOver())
            {
                frightenedMode = false;
                light.SetActive(false);
                if (tr % 2 != 0)
                {
                    scatterMode = true;
                    GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);

            }
            else
                {
                    chasingMode = true;
                    GetComponent<Pathfinding>().SearchPath(null);

                }
        }
        }
        void ChangeEnemyMode()
        {
            if (chasingMode == true)
            {
                scatterMode = true;
                chasingMode = false;
                currentPath = null;
                GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);

            }
            else
            {
                scatterMode = false;
                chasingMode = true;
                scatterPointNumber = 0;
                currentPath = null;
                GetComponent<Pathfinding>().SearchPath(null);
            }
        }



        void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<Points>())
            {
                col.gameObject.GetComponent<PlayerHealth>().MinusHealth(damageForPlayer);
                player.Respawn(); //just calling this, player itself have setting where to respawn
                                  //player.transform.position = new Vector3(positionToInstantiate.position.x, positionToInstantiate.position.y, 0);
                StartCoroutine("ChangeState");
            }
        }

        IEnumerator ChangeState()
        {
            yield return new WaitForSeconds(0.05f);
        }

        IEnumerator Wait()
        {
            waitMode = true;
            GetComponent<Pathfinding>().SearchPath(waitDotsTransform[0]);
            yield return new WaitForSeconds(timeAfterStartForEnemy);
            FindObjectOfType<wall>().SetTime();
            waitMode = false;
            if (chasingMode == true)
            {
                GetComponent<Pathfinding>().SearchPath(null);
            }
            else if (scatterMode == true)
            {
                currentPath = null;
                GetComponent<Pathfinding>().SearchPath(pathDotsTransform[0]);
                scatterPointNumber = 0;

            }

    }
        public void AppearAgain()
        {
              FindObjectOfType<wall>().SetTime();
              canMove = false;
              light.SetActive(false);
              GetComponent<BoxCollider2D>().enabled = false;
              GetComponent<SpriteRenderer>().enabled = false;
              ThrowOffSettings();
              if (gameObject.tag != "blinky")
              {
                transform.position = new Vector3(waitDotsTransform[0].position.x, waitDotsTransform[0].position.y, 0);
              }
              else
              {
                  GameObject rdot = GameObject.Find(cellPinkyStart);
                  transform.position = rdot.transform.position;
              }
               StartCoroutine(WaitForAppear());
        }
        IEnumerator WaitForAppear()
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);
            canMove = true;


    }

    private void ThrowOffSettings()
        {
            inCage = true;
            lockCellsTransform = new List<Transform>();
            waitMode = true;
            currentPath = null;  
            chasingMode = false;
            scatterMode = true;
            frightenedMode = false;
            tr = 0;
           

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "cageWall")
        {
            inCage = false;
            if (frightenedMode)
            {
                SetFrightenedMode();
            }
        }
        foreach (string cell in lockCells)
        {
            foreach (Transform child in grid.transform)
            {
                if (child.gameObject.name == cell)
                {
                    lockCellsTransform.Add(child);
                    break;
                }
            }
        }
        lockFull(lockCellsTransform);
    }

    }





