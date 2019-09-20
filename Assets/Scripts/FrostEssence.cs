using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostEssence : MonoBehaviour
{
    // type of drop (colour)
    public string type;
    private int amount;

    private GameObject playerGO;
    // Start is called before the first frame update
    void Start()
    {
        //TODO: particle effects
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float dist = Vector3.Distance(playerGO.transform.position, gameObject.transform.position);

        if(dist <= 1.5f)
        {
            transferEssence(playerGO, amount);
        }
    }


    public void transferEssence(GameObject player, int amount)
    {
        if (type == "Blue")
        {
            player.GetComponent<Player>().blueFE += amount;
            Destroy(gameObject);
        }
        else if (type == "Green")
        {
            player.GetComponent<Player>().greenFE += amount;
            Destroy(gameObject);
        }
        else
        {
            player.GetComponent<Player>().redFE += amount;
            Destroy(gameObject);
        }

    }


    public void setAmount(int value)
    {
        amount = value;
    }


}
