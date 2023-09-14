using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    [SerializeField] private float ammoLifeTime = 1f;
    [SerializeField] private GameObject attacker;
    public int ammoDamage = 1;
    
    void Start()
    {
        Destroy(gameObject, ammoLifeTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(attacker!=collision.gameObject)//確認不是發射者自撞子彈
        {
            health targetHealth = collision.gameObject.GetComponent<health>();
            if (targetHealth != null)
            {
                targetHealth.healthPoint -= ammoDamage;
            }
            //Debug.Log("reciever  "+collision.gameObject.name);Debug.Log("attacker  "+attacker);
            Destroy(gameObject, 0.05f);
        }
        
    }
    public void setAttacker(GameObject Despencer)
    {
        attacker = Despencer;
    }
}