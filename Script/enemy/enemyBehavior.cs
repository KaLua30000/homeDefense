using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyBehavior : MonoBehaviour
{
    public GameObject target;
    public Animator animator;
    public GameObject Ammo;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float attackDistance = 10f;
    [SerializeField] private float attackCD = 0.5f;
    [SerializeField] private float AmmoSpeed = 10f;
    [SerializeField] private float stateChangeDelay = 0.1f;
    private bool isHit = false;
    [SerializeField] private bool isEnemyRotate = true;
    private float timer = 0;

    private Transform EnemyGFX;

    private Path path;
    private Rigidbody2D rb;
    private Seeker seeker;
    private Transform Sprite;

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
        Sprite = transform.GetChild(0);
        //Sprite.gameObject.GetComponent<Animator>().runtimeAnimatorController = null;  //設定貼圖動畫
        EnemyGFX = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();


        InvokeRepeating("UpdatePath", 0f, 0.5f);//持續更新路徑

        currentState = State.Idle;//設定狀態機初始值
        StartCoroutine(StateMachineCoroutine());
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
    }

    IEnumerator StateMachineCoroutine() //狀態機
    {
        float standerdSpeed = speed;
        while (true)
        {
            yield return new WaitForSeconds(stateChangeDelay);
            switch (currentState)
            {
                case State.Idle:
                    //Debug.Log("Idle state");
                    break;
                case State.Move:
                    move(standerdSpeed);
                    break;
                case State.Attack:
                    attack();
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        switchState();
        FlipGFX();

        // 发射射线检测目标是否在前方
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right, transform.right, attackDistance);
        Debug.DrawRay(transform.position + transform.right, transform.right * 10);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                //Debug.Log("!!");
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }


        Vector3 v = (target.transform.position - transform.position).normalized;
        transform.right = v; //面向目標

        animator.SetFloat("SPEED", rb.velocity.magnitude);//根據速度變更動畫

        //-------抄來的，這段到底是啥，看起來沒用拔掉又會出事-----------
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        //-----------------------------------------

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //算移動到下個目標點的方向
        Vector2 force = direction * speed * Time.deltaTime; //算力量

        rb.AddForce(force);//推它

        //抄來的，每到達一個路徑點就前往下一個
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void move(float standerdSpeed)
    {
        //Debug.Log("Move state");
        speed = standerdSpeed;
    }
    void attack()
    {
        speed = 0;
        timer += stateChangeDelay;
        if(timer > attackCD)
        {
            FireAmmo(Ammo);
            timer = 0;
        }
        
    }

    void FireAmmo(GameObject ammo)//發射子彈
    {
        // 生成预制体
        GameObject spawnedPrefab = Instantiate(ammo, transform.position + transform.right, Quaternion.identity);

        // 获取生成的预制体的刚体组件
        Rigidbody2D prefabRigidbody = spawnedPrefab.GetComponent<Rigidbody2D>();

        if (prefabRigidbody != null)
        {
            // 给预制体施加向前的速度
            ammo.GetComponent<selfDestruct>().setAttacker(gameObject);
            prefabRigidbody.velocity = transform.right * AmmoSpeed;
            spawnedPrefab.transform.rotation = transform.rotation*Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void switchState()//切換狀態
    {
        //Debug.Log(currentState);
        //Debug.Log(Vector2.Distance(rb.position , target.transform.position));
        if (Vector2.Distance(rb.position, target.transform.position) <= attackDistance && isHit)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Move;
        }
    }

    void OnPathComplete(Path p)//抄來的
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FlipGFX()//根據類型翻轉或旋轉圖像
    {
        if (!isEnemyRotate)
        {
            if (transform.right.x > 0)
            {
                Sprite.transform.right = Vector3.right;
            }
            else
            {
                Sprite.transform.right = Vector3.left;
            }
        }
    }
}
