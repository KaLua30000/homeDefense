using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");//A、D或左右
        float v = Input.GetAxis("Vertical");//W、S或上下
        rb.velocity = (new Vector2(h , v)*speed);
    }
}
