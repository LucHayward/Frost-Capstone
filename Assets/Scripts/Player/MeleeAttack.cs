using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Debug.Log("Collision between staff and enemy");
        }
    }

}
