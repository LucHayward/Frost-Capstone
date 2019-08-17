using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Shield : MonoBehaviour
{
    private PlayerJ player;
    public float coolDown = 20.0f;
    public int abLevel;
    public float abTime = 0;
    public float waitTime = 5.0f;
    public float abStart;
    public float currentTime;
    public TextMeshProUGUI shieldTime;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJ>();
        shieldTime = GameObject.FindGameObjectWithTag("ShieldTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;

        if (Time.time > abTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                abStart = Time.time;
                abTime = Time.time + coolDown;
                shieldUp(player);
            }
        }

        if (currentTime - abStart > waitTime)
        {
            abStart = 0.0f;
            player.hasShield=false;
        }
        if (abTime - currentTime < 0)
        {
            shieldTime.text = "Invinsible CD: " + 0;
        }
        else
        {
            shieldTime.text = "Invinsible CD: " + (int)(abTime - currentTime);
        }
    }

    void shieldUp(PlayerJ player)
    {
        player.hasShield = true;


    }
}
