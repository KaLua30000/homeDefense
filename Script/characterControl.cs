using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;
    private Vector2 inputAxis; //輸入的WASD
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // 设置一个合适的z坐标，使得鼠标在相机视图中可见
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float angle = Mathf.Atan2(worldPosition.y-transform.position.y, worldPosition.x-transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        inputAxis = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        rb.AddForce(inputAxis*speed);

        if(Input.GetKeyDown(KeyCode.Space))
            Dash();
    }

    void Dash()
    {
        rb.AddForce(inputAxis*1000f);
    }

}
