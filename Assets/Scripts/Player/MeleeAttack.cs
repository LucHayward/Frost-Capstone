using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public int damage;
    private bool isRanged = false;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with staff");
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage, isRanged);
            Debug.Log("Collision between staff and enemy");
        }
    }
}
