using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Player;
using System;
using Enemy;

[RequireComponent(typeof(Pathfinding))]
public class EnemyAI1 : MonoBehaviour
{
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
    [SerializeField] bool waitMode = true;
    [SerializeField] bool chasingMode = false;
    [SerializeField] bool scatterMode = true;
    [SerializeField] bool frightenedMode = false;
    int scatterPointNumber = 0;
    [SerializeField] float currentTime = 0f;
    int tr = 0;
    [SerializeField] List<float> timeModes = new List<float>() { 20f, 27f, 47f, 54f, 59f };
    [SerializeField] float timerInFrightened = 5f;
    [SerializeField] GameObject light;
    [SerializeField] bool canMove = true;
    [SerializeField] bool inCage = true;
    string cellPinkyStart = "2,5";
    [SerializeField] Vector2 enemyCoords;
    List<string> lockCells = new List<string>() { "1,7", "2,7", "0,6", "0,5", "1,6", "1,5", "2,6", "2,5", "3,6", "3,5", "4,6", "4,5" };
    [SerializeField] List<Transform> lockCellsTransform = new List<Transform>();
    public event Action<List<Transform>> lockFull;

    #region get 
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

    void GetPath(List<Transform> list)
    {
        startCurrent = 0;
        currentPath = list;
    }
    #endregion
    #region get mode and set mode

    public bool GetInCage()
    {
        return inCage;
    }
    public bool IsInFrightenedMode()
    {
        return frightenedMode;
    }
    public bool IsInChasingMode()
    {
        return chasingMode;
    }

    public bool IsInScatterMode()
    {
        return scatterMode;
    }
    public void SetScatterMode()
    {
        scatterMode = true;
    }
    #endregion

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        pathfinding.newPath += GetPath;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovememt>();
        ConvertStringToTransform(patrollingPathDots, pathDotsTransform);
        ConvertStringToTransform(waitPathDots, waitDotsTransform);
        StartCoroutine("Wait");
    }

    void ConvertStringToTransform(List<string> pointDots, List<Transform> pointDotsTransform)
    {
        foreach (string dot in pointDots)
        {
            GameObject patdot = GameObject.Find(dot);
            patdot.GetComponent<Tile>().SetIsPatrollingPoint();
            patdot.GetComponent<SpriteRenderer>().color = new Color(1f, 0.95f, 0f);
            pointDotsTransform.Add(patdot.transform);
        }
    }

    public void SetFrightenedMode()
    {
        frightenedMode = true;
        light.SetActive(true);
        if (!inCage)
        {
            timerInFrightened = player.GetComponent<PlayerCollisionDetector>().GetPowerupTime();
            scatterMode = false;
            currentPath = null;
            chasingMode = false;
            GetComponent<Pathfinding>().SearchPath(null);
        }
    }


    void Update()
    {
        if (canMove)
        {
            Timers();
            ChooseMovement();
        }
    }

    void Timers()
    {
        if ((!frightenedMode && !waitMode))
        {
            Timer();
        }
        else if (frightenedMode && inCage == false)
        {
            TimerInFrightened();
        }
    }

    void ChooseMovement()
    {
        if (chasingMode && currentPath.Count != 0)
        {
            scatterMode = false;
            ChasingMovement(currentPath);
        }
        else if (chasingMode && currentPath.Count == 0)
        {
            GetComponent<Pathfinding>().SearchPath(null);
        }
        else if (currentPath.Count != 0 && scatterMode)
        {
            chasingMode = false;
            ScatterMovement(currentPath);
        }
        else if ((currentPath.Count != 0) && frightenedMode == true && inCage == false)
        {
            if (light.active == false) { light.SetActive(true); }
            ChasingMovement(currentPath);
        }

    }

    void ChasingMovement(List<Transform> startWaypoints)
    {
        if (transform.position != new Vector3(startWaypoints[0].position.x, startWaypoints[0].position.y, 0) && currentPath.Count != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, startWaypoints[0].position, enemySpeed * Time.deltaTime);
            pos = new Vector3(pos.x, pos.y, 0);
            transform.position = pos;
        }
        else
        {
            enemyCoords = new Vector2(startWaypoints[0].position.x, startWaypoints[0].position.y);
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
            SetOppositeMode();
            scatterPointNumber = 0;
            GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);
        }
        else
        {
            SetOppositeMode();
            scatterPointNumber = 0;
            startCurrent = 0;
            GetComponent<Pathfinding>().SearchPath(null);
        }
    }

    void SetOppositeMode()
    {
        scatterMode = !scatterMode;
        chasingMode = !chasingMode;
        currentPath = null;
    }

    public void StartAgain()
    {
            StopAllCoroutines();
            ThrowOffSettings(false);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = waitDotsTransform[0].position;
            inCage = true;
            if (gameObject.tag != "blinky")
            {
                startCurrent = 1;
                scatterPointNumber = 1;
            }
            else
            {
                startCurrent = 0;
                scatterPointNumber = 0;
            }
            StartCoroutine(Wait());
        }


        IEnumerator ChangeState()
        {
            yield return new WaitForSeconds(0.05f);
        }

        IEnumerator Wait()
        {
            waitMode = true;
            inCage = true;
             GetComponent<Pathfinding>().SearchPath(waitDotsTransform[startCurrent]);
            canMove = true;
            yield return new WaitForSeconds(timeAfterStartForEnemy);
            if (gameObject.tag != "blinky")
            {
                FindObjectOfType<wall>().SetTime();
            }
            else
            {
                inCage = false;
            }
            waitMode = false;
            if (scatterMode == true)
            {
                currentPath = null;
                GetComponent<Pathfinding>().SearchPath(pathDotsTransform[0]);
            }
        }

        public void AppearAgain()
        {
            ThrowOffSettings(true);
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
            yield return new WaitForSeconds(1f);
            SetSettings();
        }

        void SetSettings()
        {
            inCage = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Pathfinding>().SearchPath(pathDotsTransform[scatterPointNumber]);
            FindObjectOfType<wall>().SetTime();
            scatterMode = true;
            waitMode = false;
            canMove = true;
        }

        void ThrowOffSettings(bool turnoff)
        {
            inCage = true;

            canMove = false;
            if (turnoff)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            lockCellsTransform.Clear();
            lockFull(lockCellsTransform);
            light.SetActive(false);
            waitMode = true;
            currentPath = null;
            chasingMode = false;
            scatterMode = true;
            frightenedMode = false;
            currentTime = 0f;
             tr = 0;

        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "cageWall")
            {
                LockCells();
                inCage = false;
                if (frightenedMode)
                {
                    SetFrightenedMode();
                }
            }

        }

        void LockCells()
        {
            foreach (string cell in lockCells)
            {
                Transform lockCell = GameObject.Find(cell).transform;
                if (lockCellsTransform.Contains(lockCell)) { return; }
                lockCellsTransform.Add(lockCell);
            }
            lockFull(lockCellsTransform);
        }
    }






