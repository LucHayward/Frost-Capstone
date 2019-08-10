using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    
    public float speed;
    private Vector3 velocity;
    public int health=10;
    public TextMeshProUGUI healthText;
    public bool hasShield=false;
    public int lvl;
    public bool isFast=false;
    // Start is called before the first frame update
    void Start()
    {

        health = 10 + lvl * 2;
        

       // healthText = GameObject.FindGameObjectWithTag("HealthTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
       /* healthText.text = "Health: " + health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(7);
        }
        */
        
        
    }

/*
    private void OnTriggerEnter3D(Collider collision)
    {
        if (collision.CompareTag("BossBullet"))
        {
            if (hasShield == false)
            {
                health--;
                
            }


        }
        
    }*/

  



}
