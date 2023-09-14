using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    private GameObject healthBar;
    private characterControl CharacterControl;
    public int healthPoint = 5;
    private int fullHealthPoint;
    private float timer;

    void Start()
    {
        CharacterControl = GetComponent<characterControl>();
        fullHealthPoint = healthPoint;
        
        if(gameObject.name == "player")
        {
            healthBar = GameObject.Find("playerHealth"); //抓CANVAS
        }else{
            healthBar = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject; //抓CANVAS
        }
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthPoint/2f, 0.5f);//根據總血量調整血條大小
    }

    void Update()
    {
        Slider healthSlider = healthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
        healthSlider.value = healthPoint;   // 设置Slider的值为healthPoint
        if(gameObject.name == "player")
        {
            if(healthPoint<=0)
            {
                transform.position = new Vector2(0,0);
                CharacterControl.Alive(false);
                healthPoint = 0;
            }
            if(!CharacterControl.Alive())
            {
                timer+=Time.deltaTime;
                if(timer>=1f)
                {
                    timer = 0;
                    healthPoint++;
                }
            }
            if(healthPoint>=fullHealthPoint)
            {
                CharacterControl.Alive(true);
            }
        }
        else if(healthPoint<=0)
        {
            Destroy(gameObject, 0.1f);
        }
        else
        {
            healthBar.transform.right = Vector3.right; //把血條轉正
        }
    }
}
