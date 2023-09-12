using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    [SerializeField] private float ammoLifeTime = 1f;
    void Start()
    {
        Destroy(gameObject, ammoLifeTime);
    }
}
