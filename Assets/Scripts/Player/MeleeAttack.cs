using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public int damage;
    private bool isRanged = false;
    private int playerNum;

    public void setPlayerNum(int i)
    {
        playerNum = i;
    }
	
    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage, isRanged, playerNum);
            Debug.Log("Collision between staff and enemy");
        }
    }
}
