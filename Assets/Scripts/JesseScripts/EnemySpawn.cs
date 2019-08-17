using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemySpawn : MonoBehaviour
{
    public Boss boss;
    public Enemy enemy;
    public Transform[] spawns;
    private float timeToSpawn;
    public float spawnStartTime;
    public int numberEnemies= 0;
    public static bool isBoss = false;

    void Start()
    {
        timeToSpawn = spawnStartTime;
    }
    void Update()
    {
        if (numberEnemies < enemy.lvl * 6 + 20)


        {
            if (timeToSpawn <= 0)
            {
                
                for(int i = 0; i< SceneManager.GetActiveScene().buildIndex+1; i++)
                {
                    int randomPosition = Random.Range(0, spawns.Length - 1);
                    Instantiate(enemy, spawns[randomPosition].position, Quaternion.identity);
                    numberEnemies++;
                }
                
                timeToSpawn = spawnStartTime;
                
            }
            else
            {
                timeToSpawn -= Time.deltaTime;

            }
        }
        else if(isBoss == false)
        {
            
            Instantiate(boss, spawns[5].position, Quaternion.identity);
            isBoss = true;
           
        }

        
    }

    
}
