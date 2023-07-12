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
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    Debug.Log("Idle state");
                    yield return new WaitForSeconds(2f);
                    currentState = State.Move;
                    break;
                case State.Move:
                    Debug.Log("Move state");
                    yield return new WaitForSeconds(3f);
                    currentState = State.Attack;
                    break;
                case State.Attack:
                    Debug.Log("Attack state");
                    yield return new WaitForSeconds(1f);
                    currentState = State.Idle;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        switchState();

        FlipGFX();
        animator.SetFloat("SPEED",rb.velocity.magnitude);
        if(path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position , path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint ++ ;
        }
    }

    void switchState()
    {
        
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FlipGFX()
    {
        if(rb.velocity.x>0.0001f)
        {
            EnemyGFX.localScale = new Vector3(-1f , 1f , 1f);
        }
        else
        {
            EnemyGFX.localScale = new Vector3(1f , 1f , 1f);
        }
    }
}
