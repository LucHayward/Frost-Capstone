using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the melee attack trigger for the player
/// </summary>
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
        }
    }
}
