using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public float MainWeaponCD = 0.5f;
    public float SubWeaponCD = 4;
    private float MainWeaponCDCounter = 0;
    private float SubWeaponCDCounter = 0;
    public float moveSpeed = 5f;
    public float ammoLifeTime = 2f;
    public GameObject MainAmmo;
    public GameObject SubAmmo;
    
    void Start()
    {
        
    }

    void Update()
    {
        weaponRotation();
        MainWeaponCDCounter += Time.deltaTime;
        SubWeaponCDCounter += Time.deltaTime;

        if (Input.GetMouseButton(0) && MainWeaponCDCounter >= MainWeaponCD)
        {
            mainAttack();
            MainWeaponCDCounter = 0;
        }
        if (Input.GetMouseButtonDown(1) && SubWeaponCDCounter >= SubWeaponCD)
        {
            SubAttack();
            SubWeaponCDCounter = 0;
        }
        
    }

    void mainAttack()
    {
        FireAmmo(MainAmmo);

    }
    void SubAttack()
    {
        FireAmmo(SubAmmo);

    }

    void FireAmmo(GameObject ammo)
    {
        // 生成预制体
        GameObject spawnedPrefab = Instantiate(ammo, transform.position, Quaternion.identity);

        // 获取生成的预制体的刚体组件
        Rigidbody2D prefabRigidbody = spawnedPrefab.GetComponent<Rigidbody2D>();

        if (prefabRigidbody != null)
        {
            // 给预制体施加向前的速度
            prefabRigidbody.velocity = -transform.up * moveSpeed;
            spawnedPrefab.transform.rotation = transform.rotation;
        }
    }


    void weaponRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // 设置一个合适的z坐标，使得鼠标在相机视图中可见
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float angle = Mathf.Atan2(worldPosition.y - transform.position.y, worldPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
