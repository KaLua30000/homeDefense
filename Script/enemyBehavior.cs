using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyBehavior : MonoBehaviour
{
    public GameObject target;
    public Animator animator;
    public float speed = 200f;
    public float nextWaypointDistance = 3f ;
    public float attackDistance = 10f ;
    private bool isHit = false;
    
    private Transform EnemyGFX;

    private Path path;
    private Rigidbody2D rb;
    private Seeker seeker;

    private enum State
    {
        Idle,
        Move,
        Attack
    }
    private State currentState;

    int currentWaypoint = 0;
    bool reachedEndOfPath = false;


    void Start()
    {
        EnemyGFX = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath",0f,0.5f);

        currentState = State.Idle;//設定狀態機初始值
        StartCoroutine(StateMachineCoroutine());
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.transform.position ,OnPathComplete);
    }

    IEnumerator StateMachineCoroutine() //狀態機
    {
        float standerdSpeed = speed ;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            switch (currentState)
            {
                case State.Idle:
                    //Debug.Log("Idle state");
                    break;
                case State.Move:
                    //Debug.Log("Move state");
                    speed = standerdSpeed;
                    break;
                case State.Attack:
                    //Debug.Log("Attack state");
                    speed = 0;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        switchState();
        //FlipGFX();

        // 发射射线检测目标是否在前方
        RaycastHit2D hit = Physics2D.Raycast(transform.position+transform.right, transform.right, attackDistance);
        Debug.DrawRay(transform.position+transform.right , transform.right*10);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("!!");
                isHit = true;
            }else{
                isHit = false;
            }
        }

        
        Vector3 v = (target.transform.position - transform.position).normalized;
        transform.right = v; //面向目標

        animator.SetFloat("SPEED",rb.velocity.magnitude);//根據速度變更動畫

        //-------抄來的，這段到底是啥，看起來沒用拔掉又會出事-----------
        if(path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        //-----------------------------------------
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //算移動到下個目標點的方向
        Vector2 force = direction * speed * Time.deltaTime; //算力量

        rb.AddForce(force);//給力量

        //抄來的，每到達一個路徑點就前往下一個
        float distance = Vector2.Distance(rb.position , path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint ++ ;
        }
    }

    void switchState()//切換狀態
    {
        Debug.Log(currentState);
        //Debug.Log(Vector2.Distance(rb.position , target.transform.position));
        if(Vector2.Distance(rb.position , target.transform.position)<=attackDistance && isHit)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Move;
        }
    }

    void OnPathComplete(Path p)//抄來的，不確定功能
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FlipGFX()//翻轉圖像
    {
        if(transform.right.x>0)
        {
            EnemyGFX.localScale = new Vector3(-1f , 1f , 1f);
        }
        else
        {
            EnemyGFX.localScale = new Vector3(1f , 1f , 1f);
        }
    }
}
