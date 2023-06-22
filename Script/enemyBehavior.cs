using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyBehavior : MonoBehaviour
{
    public GameObject target;
    public Animator animator;
    public float speed;
    public float nextWaypointDistance = 3f ;
    public Transform EnemyGFX;

    private Path path;
    private Rigidbody2D rb;
    private Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;


    void Start()
    {
        speed = 200f;
        EnemyGFX = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath",0f,0.5f);
    }
    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.transform.position ,OnPathComplete);
    }


    void FixedUpdate()
    {
        FlipGFX();
        animator.SetFloat("SPEED",((rb.velocity.x)*(rb.velocity.x)+(rb.velocity.y)*(rb.velocity.y)*100f));
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
        if(rb.velocity.x>0.01f)
        {
            EnemyGFX.localScale = new Vector3(-1f , 1f , 1f);
        }
        else
        {
            EnemyGFX.localScale = new Vector3(1f , 1f , 1f);
        }
    }
}
