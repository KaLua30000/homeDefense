using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    public float speed = 20f;
    public float DashCD = 2f;
    private float DCD = 9999f;
    private Rigidbody2D rb;
    private Vector2 inputAxis; //輸入的WASD
    Vector3 centerScreenPoint;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerScreenPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));

    }


    void Update()
    {
        inputAxis = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        rb.AddForce(inputAxis*speed);

        //在此加入八方向貼圖變化

        DCD+=Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) && DCD>=DashCD) //衝刺
        {
            Dash();
            DCD = 0;
        }

        GraphicFlip();
    }

    void Dash()
    {
        rb.AddForce(inputAxis*1000f);
    }

    void GraphicFlip()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 centerScreenPoint = new Vector3(Screen.width / 4, Screen.height / 2, mousePosition.z);
        Vector3 centerOffset = mousePosition - centerScreenPoint;
        Vector3 newScale = transform.localScale;

        if (centerOffset.x >= centerScreenPoint.x)
            newScale.x = 1;
        else 
            newScale.x = -1;

        transform.localScale = newScale;
    }
}
