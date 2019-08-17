using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Ability : MonoBehaviour
{

    private Player player;
    public float coolDown=10.0f;
    public int abLevel;
    public float abTime=0;
    public float waitTime = 5.0f;
    public float abStart;
    public float currentTime;
    public TextMeshProUGUI speedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        speedTime = GameObject.FindGameObjectWithTag("SpeedTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        
        if(Time.time > abTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                abStart = Time.time;
                abTime = Time.time + coolDown;
                speedUp(player);
            }
        }

        if(currentTime - abStart > waitTime)
        {
            abStart = 0.0f;
            player.speed = 15;
            player.isFast = false;
        }
        if (abTime - currentTime < 0)
        {
            speedTime.text = "Super Speed CD: " + 0.ToString();
        }
        else
        {
            speedTime.text = "Super Speed CD: " + ((int)(abTime - currentTime)).ToString();
        }
    }

    void speedUp(Player player)
    {
        player.speed = 30;
        player.isFast = true;

    } 
}
