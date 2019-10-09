using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Mask Trap objects scripting
/// </summary>
public class MaskTrap : MonoBehaviour
{
    private float shotCD = 0.3f;
    private float currentTime;
    private float lastShotTime;

    public bool isTrap;
    public int type;

    private int counter;
    public Transform projectileSpawnPoint;

    public GameObject projectile;
    private Vector3 shotPath;

    void Start()
    {

        shotPath = gameObject.transform.forward;
        if (type == 3)
        {
            shotCD = 0.6f;
        }
    }

    /// <summary>
    /// Fires a shot on an interval
    /// </summary>
    void Update()
    {
        currentTime = Time.time;
        if (isTrap)
        {
            if (currentTime - lastShotTime > shotCD)
            {
                Shoot();
                counter++;
                if (counter != 3)
                {
                    lastShotTime = currentTime + shotCD;
                }
                else
                {
                    if (type != 3)
                    {
                        lastShotTime = currentTime + shotCD + 1.0f;
                    }
                    else
                    {
                        lastShotTime = currentTime + shotCD;
                    }

                    counter = 0;
                }

            }
        }

    }

    /// <summary>
    /// Fires the projectile
    /// </summary>
    private void Shoot()
    {
        GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;
        firedGO.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(firedGO.transform.forward, shotPath, 100f, 100f));
        firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 10, ForceMode.VelocityChange);

        Destroy(firedGO, 3);
    }
}
